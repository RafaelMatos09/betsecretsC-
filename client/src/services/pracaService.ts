import type { Bairro, Praca } from '@/types/praca'
import api from './api'

export async function listarBairros(): Promise<Bairro[]> {
  const { data } = await api.get<Bairro[]>('/api/Bairro/listar-bairros')
  return data
}

export async function listarPracas(bairroId?: number): Promise<Praca[]> {
  const { data } = await api.get<Praca[]>('/api/Praca/listar', {
    params: { bairroId: bairroId || undefined },
  })
  return data
}

export async function cadastrarPraca(praca: Praca): Promise<Praca> {
  const { data } = await api.post<Praca>('/api/Praca/cadastrar', praca)
  return data
}

export async function atualizarPraca(praca: Praca): Promise<void> {
  await api.put('/api/Praca/atualizar', praca)
}

export async function excluirPraca(id: number): Promise<void> {
  await api.delete(`/api/Praca/excluir/${id}`)
}
