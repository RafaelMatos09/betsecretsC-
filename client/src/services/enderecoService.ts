export interface EnderecoEncontrado {
  rotulo: string
  latitude: number
  longitude: number
}

interface NominatimItem {
  display_name?: string
  lat?: string
  lon?: string
}

export async function buscarEndereco(consulta: string): Promise<EnderecoEncontrado[]> {
  const url = new URL('https://nominatim.openstreetmap.org/search')
  url.searchParams.set('format', 'jsonv2')
  url.searchParams.set('q', consulta.trim())
  url.searchParams.set('countrycodes', 'br')
  url.searchParams.set('limit', '5')

  const resposta = await fetch(url, {
    headers: { Accept: 'application/json' },
  })
  if (!resposta.ok) {
    throw new Error('Não foi possível consultar o OpenStreetMap.')
  }

  const dados = (await resposta.json()) as NominatimItem[]
  return dados.flatMap((item) => {
    const latitude = Number(item.lat)
    const longitude = Number(item.lon)
    if (!item.display_name || Number.isNaN(latitude) || Number.isNaN(longitude)) return []
    return [{ rotulo: item.display_name, latitude, longitude }]
  })
}
