import axios from 'axios'
import { useCallback, useEffect, useState } from 'react'
import { MapPin, Plus, Search, Trash2 } from 'lucide-react'
import { MapaPracas } from '@/components/mapa/MapaPracas'
import { AlertDialog } from '@/components/ui/alert-dialog'
import { Button } from '@/components/ui/button'
import { ToastProvider, useToast } from '@/components/ui/toast'
import { buscarEndereco, type EnderecoEncontrado } from '@/services/enderecoService'
import * as pracaService from '@/services/pracaService'
import type { Bairro, Praca, PracaFormValues, TipoPraca } from '@/types/praca'

const TIPOS: { id: TipoPraca; label: string }[] = [
  { id: 'campo', label: 'Campo' },
  { id: 'quadra', label: 'Quadra' },
  { id: 'praca', label: 'Praça' },
]

const inputClass = 'h-10 w-full rounded-lg border border-input bg-background px-3 text-sm'

function vazio(bairroId: number | '' = ''): PracaFormValues {
  return {
    bairroId,
    nome: '',
    endereco: '',
    tipo: 'campo',
    latitude: null,
    longitude: null,
    observacoes: '',
  }
}

function errorMessage(err: unknown, fallback: string) {
  if (axios.isAxiosError(err)) {
    const data = err.response?.data as { message?: string } | undefined
    return data?.message || err.message || fallback
  }
  return fallback
}

function tipoLabel(tipo?: string | null) {
  return TIPOS.find((item) => item.id === tipo)?.label ?? 'Campo'
}

export default function PracasRoute() {
  return (
    <ToastProvider>
      <PracasPage />
    </ToastProvider>
  )
}

function PracasPage() {
  const { toast } = useToast()
  const [bairros, setBairros] = useState<Bairro[]>([])
  const [pracas, setPracas] = useState<Praca[]>([])
  const [filtroBairro, setFiltroBairro] = useState<number | ''>('')
  const [form, setForm] = useState<PracaFormValues>(vazio)
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [pendingDelete, setPendingDelete] = useState<Praca | null>(null)
  const [busca, setBusca] = useState('')
  const [resultados, setResultados] = useState<EnderecoEncontrado[]>([])
  const [buscando, setBuscando] = useState(false)

  const load = useCallback(async () => {
    setLoading(true)
    setError(null)
    try {
      const [lista, listaBairros] = await Promise.all([
        pracaService.listarPracas(typeof filtroBairro === 'number' ? filtroBairro : undefined),
        pracaService.listarBairros().catch(() => [] as Bairro[]),
      ])
      setPracas(lista)
      setBairros(listaBairros)
    } catch (err) {
      setError(errorMessage(err, 'Não foi possível carregar as praças.'))
    } finally {
      setLoading(false)
    }
  }, [filtroBairro])

  useEffect(() => {
    void load()
  }, [load])

  function escolher(praca: Praca) {
    setForm({
      id: praca.id,
      bairroId: praca.bairroId ?? '',
      nome: praca.nome,
      endereco: praca.endereco ?? '',
      tipo: (praca.tipo as TipoPraca) || 'campo',
      latitude: Number(praca.latitude),
      longitude: Number(praca.longitude),
      observacoes: praca.observacoes ?? '',
    })
  }

  async function pesquisarEndereco() {
    const consulta = busca.trim()
    if (consulta.length < 3) {
      toast('Escreva a praça e o bairro. Exemplo: praça do Encantado, Rio de Janeiro.', 'error')
      return
    }
    setBuscando(true)
    try {
      const lista = await buscarEndereco(consulta)
      setResultados(lista)
      if (lista.length === 0) toast('Nenhum lugar encontrado no OpenStreetMap.', 'error')
    } catch (err) {
      const mensagem = err instanceof Error ? err.message : errorMessage(err, 'Não foi possível consultar o OpenStreetMap.')
      toast(mensagem, 'error')
    } finally {
      setBuscando(false)
    }
  }

  function usarEndereco(item: EnderecoEncontrado) {
    setForm((atual) => ({
      ...atual,
      latitude: item.latitude,
      longitude: item.longitude,
      endereco: atual.endereco || item.rotulo,
      nome: atual.nome || item.rotulo.split(',')[0]?.trim() || atual.nome,
    }))
    setResultados([])
    setBusca(item.rotulo)
  }

  function marcarPonto(coords: { latitude: number; longitude: number }) {
    setForm((atual) => ({
      ...atual,
      latitude: coords.latitude,
      longitude: coords.longitude,
    }))
  }

  async function salvar() {
    if (!form.nome.trim() || form.latitude == null || form.longitude == null) {
      toast('Informe o nome e marque o ponto no mapa.', 'error')
      return
    }

    const payload: Praca = {
      id: form.id,
      bairroId: form.bairroId === '' ? null : Number(form.bairroId),
      nome: form.nome.trim(),
      endereco: form.endereco.trim() || null,
      tipo: form.tipo,
      latitude: form.latitude,
      longitude: form.longitude,
      observacoes: form.observacoes.trim() || null,
    }

    setSaving(true)
    try {
      if (payload.id) {
        await pracaService.atualizarPraca(payload)
        toast('Campo atualizado.')
      } else {
        await pracaService.cadastrarPraca(payload)
        toast('Campo adicionado ao mapa.')
      }
      setForm(vazio(filtroBairro))
      await load()
    } catch (err) {
      toast(errorMessage(err, 'Não foi possível salvar o campo.'), 'error')
    } finally {
      setSaving(false)
    }
  }

  async function excluir(praca: Praca) {
    if (!praca.id) return
    setSaving(true)
    try {
      await pracaService.excluirPraca(praca.id)
      if (form.id === praca.id) setForm(vazio(filtroBairro))
      toast('Campo removido do mapa.')
      await load()
    } catch (err) {
      toast(errorMessage(err, 'Não foi possível excluir o campo.'), 'error')
    } finally {
      setSaving(false)
      setPendingDelete(null)
    }
  }

  const pontoNovo =
    form.latitude == null || form.longitude == null
      ? null
      : { latitude: form.latitude, longitude: form.longitude }

  return (
    <div className="space-y-4">
      <div className="flex flex-col gap-3 rounded-2xl border border-border bg-card p-4 shadow-sm md:flex-row md:items-end md:justify-between">
        <div>
          <p className="font-mono text-[10px] uppercase tracking-[0.2em] text-muted-foreground">Bairro</p>
          <h2 className="font-display text-3xl font-bold text-emerald-950">Praças e campos</h2>
          <p className="mt-1 max-w-xl text-sm text-muted-foreground">
            O mapa é o OpenStreetMap. Busque a praça ou clique no ponto para gravar o campo do jogo.
          </p>
        </div>
        <label className="text-sm md:w-64">
          <span className="mb-1 block font-mono text-[10px] uppercase tracking-widest text-muted-foreground">Filtrar bairro</span>
          <select
            className={inputClass}
            value={filtroBairro}
            onChange={(event) => setFiltroBairro(event.target.value ? Number(event.target.value) : '')}
          >
            <option value="">Todos os bairros</option>
            {bairros.map((bairro) => (
              <option key={bairro.id} value={bairro.id}>
                {bairro.nome}
                {bairro.cidade ? ` · ${bairro.cidade}` : ''}
              </option>
            ))}
          </select>
        </label>
      </div>

      {error ? (
        <div className="rounded-2xl border border-destructive/30 bg-card p-6">
          <p className="font-semibold text-destructive">{error}</p>
          <button type="button" className="mt-3 text-sm underline" onClick={() => void load()}>
            Tentar novamente
          </button>
        </div>
      ) : (
        <div className="grid gap-4 xl:grid-cols-[minmax(0,1.4fr)_minmax(320px,0.8fr)]">
          <section className="space-y-3">
            <form
              className="grid gap-2"
              onSubmit={(event) => {
                event.preventDefault()
                void pesquisarEndereco()
              }}
            >
              <label className="text-sm">
                <span className="mb-1 block font-mono text-[10px] uppercase tracking-widest text-muted-foreground">
                  Buscar no OpenStreetMap
                </span>
                <span className="flex gap-2">
                  <input
                    className={inputClass}
                    value={busca}
                    onChange={(event) => setBusca(event.target.value)}
                    placeholder="Praça do Encantado, Rio de Janeiro"
                  />
                  <Button type="submit" variant="outline" disabled={buscando}>
                    <Search className="size-4" />
                    {buscando ? 'Buscando...' : 'Buscar'}
                  </Button>
                </span>
              </label>
              {resultados.length > 0 && (
                <div className="overflow-hidden rounded-xl border border-border bg-card">
                  {resultados.map((item) => (
                    <button
                      key={`${item.latitude}-${item.longitude}`}
                      type="button"
                      className="block w-full border-b border-border px-3 py-2 text-left text-sm last:border-b-0 hover:bg-emerald-50"
                      onClick={() => usarEndereco(item)}
                    >
                      {item.rotulo}
                    </button>
                  ))}
                </div>
              )}
            </form>
            <MapaPracas
              pracas={pracas}
              selectedId={form.id}
              pick={pontoNovo}
              onSelect={escolher}
              onPick={marcarPonto}
            />
            <p className="text-sm text-muted-foreground">
              {loading ? 'Carregando campos...' : `${pracas.length} campo${pracas.length === 1 ? '' : 's'} no mapa`}
            </p>
          </section>

          <section className="space-y-4">
            <form
              className="grid gap-3 rounded-2xl border border-border bg-card p-4 shadow-sm"
              onSubmit={(event) => {
                event.preventDefault()
                void salvar()
              }}
            >
              <div className="flex items-center justify-between gap-3">
                <div>
                  <p className="font-mono text-[10px] uppercase tracking-widest text-muted-foreground">
                    {form.id ? 'Editar campo' : 'Novo campo'}
                  </p>
                  <h3 className="font-display text-xl font-bold">{form.nome || 'Ponto no mapa'}</h3>
                </div>
                <Button type="button" variant="outline" size="sm" onClick={() => setForm(vazio(filtroBairro))}>
                  <Plus className="size-3.5" />
                  Novo
                </Button>
              </div>

              <label className="text-sm">
                <span className="mb-1 block font-medium">Nome</span>
                <input
                  className={inputClass}
                  value={form.nome}
                  onChange={(event) => setForm((atual) => ({ ...atual, nome: event.target.value }))}
                  placeholder="Campinho da praça"
                  maxLength={150}
                  required
                />
              </label>

              <div className="grid gap-3 sm:grid-cols-2">
                <label className="text-sm">
                  <span className="mb-1 block font-medium">Tipo</span>
                  <select
                    className={inputClass}
                    value={form.tipo}
                    onChange={(event) => setForm((atual) => ({ ...atual, tipo: event.target.value as TipoPraca }))}
                  >
                    {TIPOS.map((tipo) => (
                      <option key={tipo.id} value={tipo.id}>
                        {tipo.label}
                      </option>
                    ))}
                  </select>
                </label>
                <label className="text-sm">
                  <span className="mb-1 block font-medium">Bairro</span>
                  <select
                    className={inputClass}
                    value={form.bairroId}
                    onChange={(event) =>
                      setForm((atual) => ({
                        ...atual,
                        bairroId: event.target.value ? Number(event.target.value) : '',
                      }))
                    }
                  >
                    <option value="">Sem bairro</option>
                    {bairros.map((bairro) => (
                      <option key={bairro.id} value={bairro.id}>
                        {bairro.nome}
                      </option>
                    ))}
                  </select>
                </label>
              </div>

              <label className="text-sm">
                <span className="mb-1 block font-medium">Endereço</span>
                <input
                  className={inputClass}
                  value={form.endereco}
                  onChange={(event) => setForm((atual) => ({ ...atual, endereco: event.target.value }))}
                  placeholder="Rua, referência, lado da quadra"
                  maxLength={250}
                />
              </label>

              <div className="grid grid-cols-2 gap-3">
                <label className="text-sm">
                  <span className="mb-1 block font-medium">Latitude</span>
                  <input
                    className={inputClass}
                    inputMode="decimal"
                    value={form.latitude ?? ''}
                    onChange={(event) =>
                      setForm((atual) => ({
                        ...atual,
                        latitude: event.target.value === '' ? null : Number(event.target.value),
                      }))
                    }
                    placeholder="-23.55"
                    required
                  />
                </label>
                <label className="text-sm">
                  <span className="mb-1 block font-medium">Longitude</span>
                  <input
                    className={inputClass}
                    inputMode="decimal"
                    value={form.longitude ?? ''}
                    onChange={(event) =>
                      setForm((atual) => ({
                        ...atual,
                        longitude: event.target.value === '' ? null : Number(event.target.value),
                      }))
                    }
                    placeholder="-46.63"
                    required
                  />
                </label>
              </div>

              <label className="text-sm">
                <span className="mb-1 block font-medium">Observações</span>
                <textarea
                  className="min-h-16 w-full rounded-lg border border-input bg-background px-3 py-2 text-sm"
                  value={form.observacoes}
                  onChange={(event) => setForm((atual) => ({ ...atual, observacoes: event.target.value }))}
                  placeholder="Iluminação, trave, horário livre..."
                />
              </label>

              <Button type="submit" disabled={saving}>
                {saving ? 'Salvando...' : form.id ? 'Atualizar campo' : 'Salvar no mapa'}
              </Button>
            </form>

            <div className="grid gap-2">
              {pracas.map((praca) => {
                const ativa = form.id === praca.id
                const jogos = praca.jogosMarcados ?? 0
                return (
                  <article
                    key={praca.id}
                    className={`rounded-2xl border p-3 ${ativa ? 'border-emerald-700 bg-emerald-50' : 'border-border bg-card'}`}
                  >
                    <button type="button" className="w-full text-left" onClick={() => escolher(praca)}>
                      <div className="flex items-start justify-between gap-3">
                        <div>
                          <p className="font-display text-lg font-bold">{praca.nome}</p>
                          <p className="font-mono text-[10px] uppercase tracking-widest text-muted-foreground">
                            {tipoLabel(praca.tipo)}
                            {praca.bairroNome ? ` · ${praca.bairroNome}` : ''}
                          </p>
                        </div>
                        <span className="inline-flex items-center gap-1 rounded-full bg-emerald-950 px-2 py-1 font-mono text-[10px] uppercase tracking-widest text-amber-100">
                          <MapPin className="size-3" />
                          {jogos}
                        </span>
                      </div>
                      {praca.endereco && <p className="mt-1 text-sm text-muted-foreground">{praca.endereco}</p>}
                    </button>
                    <div className="mt-2 flex justify-end">
                      <Button size="sm" variant="destructive" disabled={saving} onClick={() => setPendingDelete(praca)}>
                        <Trash2 className="size-3.5" />
                        Excluir
                      </Button>
                    </div>
                  </article>
                )
              })}
              {!loading && pracas.length === 0 && (
                <div className="rounded-2xl border border-dashed border-emerald-900/20 bg-emerald-50/40 px-4 py-8 text-center">
                  <p className="font-display text-xl font-bold text-emerald-950">Nenhum campo marcado</p>
                  <p className="mt-1 text-sm text-muted-foreground">
                    Clique no mapa ou informe latitude e longitude para salvar a primeira praça.
                  </p>
                </div>
              )}
            </div>
          </section>
        </div>
      )}

      <AlertDialog
        open={Boolean(pendingDelete)}
        title="Excluir campo"
        description={`Remover ${pendingDelete?.nome ?? 'este campo'} do mapa? Os jogos já marcados continuam na agenda.`}
        confirmLabel="Excluir"
        loading={saving}
        onCancel={() => setPendingDelete(null)}
        onConfirm={() => {
          if (pendingDelete) void excluir(pendingDelete)
        }}
      />
    </div>
  )
}
