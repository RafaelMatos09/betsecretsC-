export type TipoPraca = 'campo' | 'quadra' | 'praca'

export interface Bairro {
  id: number
  nome: string
  cidade?: string | null
  estado?: string | null
}

export interface Praca {
  id?: number
  bairroId?: number | null
  nome: string
  endereco?: string | null
  tipo?: TipoPraca | string | null
  latitude: number
  longitude: number
  observacoes?: string | null
  bairroNome?: string | null
  jogosMarcados?: number
}

export interface PracaFormValues {
  id?: number
  bairroId: number | ''
  nome: string
  endereco: string
  tipo: TipoPraca
  latitude: number | null
  longitude: number | null
  observacoes: string
}
