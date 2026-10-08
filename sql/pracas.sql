-- Praças e campos do bairro, ligados aos jogos do calendário.
-- A API também aplica este esquema ao subir (Schema/PracasSchema.cs).

CREATE TABLE IF NOT EXISTS pracas (
    id BIGSERIAL PRIMARY KEY,
    bairro_id BIGINT,
    nome VARCHAR(150) NOT NULL,
    endereco VARCHAR(250),
    tipo VARCHAR(40) NOT NULL DEFAULT 'campo',
    latitude DOUBLE PRECISION NOT NULL,
    longitude DOUBLE PRECISION NOT NULL,
    observacoes TEXT,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

ALTER TABLE calendario_jogos
    ADD COLUMN IF NOT EXISTS praca_id BIGINT REFERENCES pracas(id) ON DELETE SET NULL;

CREATE INDEX IF NOT EXISTS idx_pracas_bairro ON pracas (bairro_id);
CREATE INDEX IF NOT EXISTS idx_calendario_jogos_praca ON calendario_jogos (praca_id);
