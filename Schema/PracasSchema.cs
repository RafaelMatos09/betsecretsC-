using betsecrets.ORM;

namespace betsecrets.Schema
{
    public static class PracasSchema
    {
        public static async Task AplicarAsync(AppDbContext db)
        {
            await db.ExecuteAsync(@"
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
                )");

            var tipoBairro = await db.ExecuteScalarAsync<string?>(@"
                SELECT data_type
                FROM information_schema.columns
                WHERE table_schema = 'public'
                  AND table_name = 'bairros'
                  AND column_name = 'id'");

            var tipoColuna = await db.ExecuteScalarAsync<string?>(@"
                SELECT data_type
                FROM information_schema.columns
                WHERE table_schema = 'public'
                  AND table_name = 'pracas'
                  AND column_name = 'bairro_id'");

            if (tipoBairro == "integer" && tipoColuna == "bigint")
            {
                await db.ExecuteAsync(
                    "ALTER TABLE pracas ALTER COLUMN bairro_id TYPE INTEGER USING bairro_id::integer");
            }

            try
            {
                await db.ExecuteAsync(@"
                    DO $$
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_pracas_bairro')
                           AND EXISTS (
                               SELECT 1 FROM information_schema.tables
                               WHERE table_schema = 'public' AND table_name = 'bairros'
                           ) THEN
                            ALTER TABLE pracas
                                ADD CONSTRAINT fk_pracas_bairro
                                FOREIGN KEY (bairro_id) REFERENCES bairros(id) ON DELETE SET NULL;
                        END IF;
                    END $$;");
            }
            catch (Exception)
            {
                // O id do bairro continua gravado mesmo quando o tipo da coluna impede a chave estrangeira.
            }

            await db.ExecuteAsync(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM information_schema.columns
                        WHERE table_schema = 'public'
                          AND table_name = 'calendario_jogos'
                          AND column_name = 'praca_id'
                    ) THEN
                        ALTER TABLE calendario_jogos
                            ADD COLUMN praca_id BIGINT REFERENCES pracas(id) ON DELETE SET NULL;
                    END IF;
                END $$;");

            await db.ExecuteAsync("CREATE INDEX IF NOT EXISTS idx_pracas_bairro ON pracas (bairro_id)");
            await db.ExecuteAsync(
                "CREATE INDEX IF NOT EXISTS idx_calendario_jogos_praca ON calendario_jogos (praca_id)");
        }
    }
}
