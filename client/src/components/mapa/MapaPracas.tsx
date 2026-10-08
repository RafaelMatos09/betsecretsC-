import { useEffect, useRef } from 'react'
import { divIcon, DomEvent, latLngBounds, map as createMap, marker, tileLayer, type Map as LeafletMap, type Marker } from 'leaflet'
import 'leaflet/dist/leaflet.css'
import type { Praca } from '@/types/praca'

interface MapaPracasProps {
  pracas: Praca[]
  selectedId?: number | null
  onSelect?: (praca: Praca) => void
  onPick?: (coords: { latitude: number; longitude: number }) => void
  pick?: { latitude: number; longitude: number } | null
  className?: string
}

const CENTRO = { lat: -22.892, lng: -43.306 }

function escapeHtml(value: string) {
  return value.replace(/[&<>"']/g, (char) => {
    const mapa: Record<string, string> = {
      '&': '&amp;',
      '<': '&lt;',
      '>': '&gt;',
      '"': '&quot;',
      "'": '&#39;',
    }
    return mapa[char] ?? char
  })
}

function pino(cor: string, tamanho: number) {
  return divIcon({
    className: 'mapa-pino',
    iconSize: [tamanho, tamanho],
    iconAnchor: [tamanho / 2, tamanho / 2],
    html: `<span style="display:block;width:${tamanho}px;height:${tamanho}px;border-radius:9999px;background:${cor};border:2px solid #fde68a;box-shadow:0 1px 4px rgb(0 0 0 / 35%)"></span>`,
  })
}

function mesmoPonto(praca: Praca, pick?: { latitude: number; longitude: number } | null) {
  if (!pick || praca.latitude == null || praca.longitude == null) return false
  return Math.abs(praca.latitude - pick.latitude) < 0.00001 && Math.abs(praca.longitude - pick.longitude) < 0.00001
}

export function MapaPracas({ pracas, selectedId, onSelect, onPick, pick, className }: MapaPracasProps) {
  const containerRef = useRef<HTMLDivElement>(null)
  const mapRef = useRef<LeafletMap | null>(null)
  const markersRef = useRef<Marker[]>([])
  const pickMarkerRef = useRef<Marker | null>(null)
  const onSelectRef = useRef(onSelect)
  const onPickRef = useRef(onPick)
  const ultimoEnquadramento = useRef('')

  onSelectRef.current = onSelect
  onPickRef.current = onPick

  useEffect(() => {
    const elemento = containerRef.current
    if (!elemento || mapRef.current) return

    const mapa = createMap(elemento, {
      center: [CENTRO.lat, CENTRO.lng],
      zoom: 14,
      zoomControl: true,
    })
    tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 19,
      attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>',
    }).addTo(mapa)
    mapa.on('click', (evento) => {
      if (!onPickRef.current) return
      onPickRef.current({
        latitude: evento.latlng.lat,
        longitude: evento.latlng.lng,
      })
    })
    mapRef.current = mapa
    window.setTimeout(() => mapa.invalidateSize(), 0)

    return () => {
      mapa.remove()
      mapRef.current = null
      markersRef.current = []
      pickMarkerRef.current = null
    }
  }, [])

  useEffect(() => {
    const mapa = mapRef.current
    if (!mapa) return

    markersRef.current.forEach((item) => item.remove())
    markersRef.current = []

    const limites = latLngBounds([])
    let quantidade = 0

    for (const praca of pracas) {
      if (praca.latitude == null || praca.longitude == null) continue
      const selecionada = praca.id === selectedId
      const ponto = marker([Number(praca.latitude), Number(praca.longitude)], {
        title: praca.nome,
        zIndexOffset: selecionada ? 200 : 0,
        icon: pino(selecionada ? '#d97706' : '#064e3b', selecionada ? 22 : 16),
      })
      ponto.on('click', (evento) => {
        DomEvent.stopPropagation(evento)
        onSelectRef.current?.(praca)
        const jogos = praca.jogosMarcados ?? 0
        const detalhe = jogos === 1 ? '1 jogo marcado' : `${jogos} jogos marcados`
        ponto.bindPopup(
          `<div style="font-family:sans-serif;min-width:140px"><strong>${escapeHtml(praca.nome)}</strong><div style="margin-top:4px;color:#57534e;font-size:12px">${escapeHtml(praca.endereco || detalhe)}</div></div>`,
        )
        ponto.openPopup()
      })
      ponto.addTo(mapa)
      markersRef.current.push(ponto)
      limites.extend([Number(praca.latitude), Number(praca.longitude)])
      quantidade += 1
    }

    const chave = pracas.map((item) => `${item.id}:${item.latitude}:${item.longitude}`).join('|')
    if (quantidade > 0 && ultimoEnquadramento.current !== chave && limites.isValid()) {
      if (quantidade === 1) {
        mapa.setView(limites.getCenter(), 16)
      } else {
        mapa.fitBounds(limites, { padding: [48, 48] })
      }
      ultimoEnquadramento.current = chave
    }

    const selecionada = pracas.find((item) => item.id === selectedId)
    if (selecionada?.latitude != null && selecionada.longitude != null) {
      mapa.panTo([Number(selecionada.latitude), Number(selecionada.longitude)])
    }
  }, [pracas, selectedId])

  useEffect(() => {
    const mapa = mapRef.current
    if (!mapa) return

    pickMarkerRef.current?.remove()
    pickMarkerRef.current = null
    if (!pick || pracas.some((item) => item.id === selectedId && mesmoPonto(item, pick))) return

    const ponto = marker([pick.latitude, pick.longitude], {
      title: 'Novo campo',
      zIndexOffset: 400,
      icon: pino('#f59e0b', 20),
    }).addTo(mapa)
    pickMarkerRef.current = ponto
    mapa.panTo([pick.latitude, pick.longitude])
  }, [pick, pracas, selectedId])

  return (
    <div className={`mapa-osm relative isolate overflow-hidden rounded-2xl border border-emerald-950/10 bg-emerald-50 ${className ?? ''}`}>
      <div ref={containerRef} className="h-72 w-full" />
      {onPick && (
        <p className="pointer-events-none absolute bottom-3 left-3 z-[5] rounded-full bg-emerald-950/90 px-3 py-1 font-mono text-[10px] uppercase tracking-widest text-amber-100">
          Clique no mapa para marcar o campo
        </p>
      )}
    </div>
  )
}
