/* =====================================================================
   Alejandria_DB - Alinear la base con la documentación (Entrega 3)

   1. Membresía prepaga: se elimina el modelo en cuotas (tabla Cuota y
      columna Pago.IdCuota). La documentación (G03, CUN-01, CUN-03) define
      que la membresía vencida no genera deuda.
   2. Ejemplar.Valor: valor monetario de cada ejemplar, usado en el
      informe de inventario y patrimonio (G02 RFN2, CUN-10).
   3. Parámetro MotivoDeterioroEnAuditoria (CUN-06 paso 3, estado observado).

   Después de ejecutarlo hay que entrar como administrador y usar
   "Recalcular Digito Verificadores": el DVH del ejemplar ahora incluye
   el valor.
   ===================================================================== */

USE Alejandria_DB;
GO

BACKUP DATABASE Alejandria_DB
    TO DISK = N'Alejandria_DB_antes_coherencia.bak'
    WITH INIT, NAME = N'Alejandria_DB antes de alinear con la documentación';
GO

SET XACT_ABORT ON;
BEGIN TRANSACTION;

/* ---------------------------------------------------------------------
   1. Sin cuotas
   --------------------------------------------------------------------- */
IF COL_LENGTH('dbo.Pago', 'IdCuota') IS NOT NULL
BEGIN
    DECLARE @fk sysname, @sql nvarchar(400);
    SELECT @fk = fk.name
    FROM sys.foreign_keys fk
    JOIN sys.foreign_key_columns fc ON fc.constraint_object_id = fk.object_id
    WHERE fk.parent_object_id = OBJECT_ID('dbo.Pago')
      AND COL_NAME(fc.parent_object_id, fc.parent_column_id) = 'IdCuota';

    IF @fk IS NOT NULL
    BEGIN
        SET @sql = N'ALTER TABLE dbo.Pago DROP CONSTRAINT ' + QUOTENAME(@fk) + N';';
        EXEC sp_executesql @sql;
    END

    ALTER TABLE dbo.Pago DROP COLUMN IdCuota;
END

IF OBJECT_ID('dbo.Cuota', 'U') IS NOT NULL
    DROP TABLE dbo.Cuota;

DELETE FROM dbo.ControlDVV WHERE NombreTabla = 'Cuota';

/* ---------------------------------------------------------------------
   2. Valor del ejemplar
   --------------------------------------------------------------------- */
IF COL_LENGTH('dbo.Ejemplar', 'Valor') IS NULL
BEGIN
    ALTER TABLE dbo.Ejemplar
        ADD Valor DECIMAL(12, 2) NOT NULL
            CONSTRAINT DF_Ejemplar_Valor DEFAULT (0)
            CONSTRAINT CK_Ejemplar_Valor CHECK (Valor >= 0);
END

/* ---------------------------------------------------------------------
   3. Parámetro del motivo de deterioro detectado en auditoría
   --------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM dbo.Parametro WHERE Nombre = 'MotivoDeterioroEnAuditoria')
    INSERT INTO dbo.Parametro (Nombre, Valor, Descripcion)
    VALUES ('MotivoDeterioroEnAuditoria', N'Deterioro observado en auditoría',
            N'Motivo que se registra al marcar como deteriorado un ejemplar durante una auditoría.');

COMMIT TRANSACTION;
GO

/* Verificación */
SELECT 'Pago.IdCuota' AS Elemento, CASE WHEN COL_LENGTH('dbo.Pago', 'IdCuota') IS NULL THEN 'eliminada' ELSE 'TODAVIA EXISTE' END AS Estado
UNION ALL SELECT 'Tabla Cuota', CASE WHEN OBJECT_ID('dbo.Cuota', 'U') IS NULL THEN 'eliminada' ELSE 'TODAVIA EXISTE' END
UNION ALL SELECT 'Ejemplar.Valor', CASE WHEN COL_LENGTH('dbo.Ejemplar', 'Valor') IS NOT NULL THEN 'creada' ELSE 'FALTA' END
UNION ALL SELECT 'Parametro MotivoDeterioroEnAuditoria', CASE WHEN EXISTS (SELECT 1 FROM dbo.Parametro WHERE Nombre = 'MotivoDeterioroEnAuditoria') THEN 'cargado' ELSE 'FALTA' END;
GO
