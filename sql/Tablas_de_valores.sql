/* =====================================================================
   Alejandria - Tablas de valores y parametros
   ---------------------------------------------------------------------
   Saca de las BLL las listas fijas de valores y las pasa a tablas:
     - EstadoEjemplar, EstadoEjemplarTransicion, OrigenEjemplar
     - TipoMovimientoEjemplar
     - ResultadoDiferencia, ResolucionDiferencia
     - MedioDePago
     - Parametro (nombre / valor)
   Y cambia las columnas de texto por claves foraneas:
     Ejemplar.Estado/Origen                 -> IdEstado / IdOrigen
     EjemplarMovimiento.Tipo/EstadoAnterior/EstadoNuevo
                                            -> IdTipoMovimiento / IdEstadoAnterior / IdEstadoNuevo
     AuditoriaDiferencia.Resultado/Resolucion -> IdResultado / IdResolucion (NULL = pendiente)
     Pago.MedioDePago                       -> IdMedioDePago

   Los datos existentes se conservan. Los DVH y DVV no cambian, porque
   se siguen calculando con el nombre del valor (Bueno, Compra, ...).
   El script se puede volver a ejecutar: cada paso se salta si ya se hizo.
   ===================================================================== */

USE Alejandria_DB;
GO

/* 0. Copia de seguridad antes de tocar nada (queda en la carpeta de backups de SQL Server). */
BACKUP DATABASE Alejandria_DB TO DISK = N'Alejandria_DB_antes_tablas_de_valores.bak' WITH INIT, NAME = N'Antes de tablas de valores';
GO

/* Procedimiento temporal: borra una columna junto con sus DEFAULT, CHECK e indices. */
IF OBJECT_ID('tempdb..#QuitarColumna') IS NOT NULL DROP PROCEDURE #QuitarColumna;
GO
CREATE PROCEDURE #QuitarColumna @tabla SYSNAME, @columna SYSNAME
AS
BEGIN
    IF COL_LENGTH(@tabla, @columna) IS NULL RETURN;

    DECLARE @sql NVARCHAR(MAX) = N'';
    DECLARE @idTabla INT = OBJECT_ID(@tabla);
    DECLARE @idColumna INT = COLUMNPROPERTY(@idTabla, @columna, 'ColumnId');

    SELECT @sql += N'ALTER TABLE ' + @tabla + N' DROP CONSTRAINT ' + QUOTENAME(name) + N';'
    FROM sys.default_constraints
    WHERE parent_object_id = @idTabla AND parent_column_id = @idColumna;

    SELECT @sql += N'ALTER TABLE ' + @tabla + N' DROP CONSTRAINT ' + QUOTENAME(name) + N';'
    FROM sys.check_constraints
    WHERE parent_object_id = @idTabla
      AND (parent_column_id = @idColumna OR CHARINDEX(QUOTENAME(@columna), definition) > 0);

    SELECT @sql += N'DROP INDEX ' + QUOTENAME(i.name) + N' ON ' + @tabla + N';'
    FROM sys.indexes i
    WHERE i.object_id = @idTabla AND i.is_primary_key = 0 AND i.is_unique_constraint = 0
      AND EXISTS (SELECT 1 FROM sys.index_columns ic
                  WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.column_id = @idColumna);

    SET @sql += N'ALTER TABLE ' + @tabla + N' DROP COLUMN ' + QUOTENAME(@columna) + N';';
    EXEC sp_executesql @sql;
END
GO

/* =====================================================================
   1. Tablas de valores
   ===================================================================== */

IF OBJECT_ID('dbo.EstadoEjemplar', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.EstadoEjemplar (
        IdEstado INT         NOT NULL CONSTRAINT PK_EstadoEjemplar PRIMARY KEY,
        Nombre   VARCHAR(20) NOT NULL CONSTRAINT UQ_EstadoEjemplar_Nombre UNIQUE
    );
    INSERT INTO dbo.EstadoEjemplar (IdEstado, Nombre) VALUES
        (1, 'Bueno'), (2, 'Deteriorado'), (3, 'EnRestauracion'), (4, 'Baja');
END
GO

IF OBJECT_ID('dbo.EstadoEjemplarTransicion', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.EstadoEjemplarTransicion (
        IdEstadoOrigen  INT NOT NULL,
        IdEstadoDestino INT NOT NULL,
        CONSTRAINT PK_EstadoEjemplarTransicion PRIMARY KEY (IdEstadoOrigen, IdEstadoDestino),
        CONSTRAINT FK_EstadoEjemplarTransicion_Origen  FOREIGN KEY (IdEstadoOrigen)  REFERENCES dbo.EstadoEjemplar (IdEstado),
        CONSTRAINT FK_EstadoEjemplarTransicion_Destino FOREIGN KEY (IdEstadoDestino) REFERENCES dbo.EstadoEjemplar (IdEstado)
    );
    -- Ciclo de vida: Bueno -> Deteriorado -> En restauracion -> Bueno, y cualquiera -> Baja (la Baja no tiene salida).
    INSERT INTO dbo.EstadoEjemplarTransicion (IdEstadoOrigen, IdEstadoDestino) VALUES
        (1, 2), (1, 4),
        (2, 3), (2, 4),
        (3, 1), (3, 4);
END
GO

IF OBJECT_ID('dbo.OrigenEjemplar', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.OrigenEjemplar (
        IdOrigen INT         NOT NULL CONSTRAINT PK_OrigenEjemplar PRIMARY KEY,
        Nombre   VARCHAR(20) NOT NULL CONSTRAINT UQ_OrigenEjemplar_Nombre UNIQUE
    );
    INSERT INTO dbo.OrigenEjemplar (IdOrigen, Nombre) VALUES (1, 'Compra'), (2, 'Donacion');
END
GO

IF OBJECT_ID('dbo.TipoMovimientoEjemplar', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.TipoMovimientoEjemplar (
        IdTipoMovimiento INT         NOT NULL CONSTRAINT PK_TipoMovimientoEjemplar PRIMARY KEY,
        Nombre           VARCHAR(20) NOT NULL CONSTRAINT UQ_TipoMovimientoEjemplar_Nombre UNIQUE
    );
    INSERT INTO dbo.TipoMovimientoEjemplar (IdTipoMovimiento, Nombre) VALUES
        (1, 'Ingreso'), (2, 'Ubicacion'), (3, 'CambioEstado'), (4, 'Baja');
END
GO

IF OBJECT_ID('dbo.ResultadoDiferencia', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ResultadoDiferencia (
        IdResultado INT         NOT NULL CONSTRAINT PK_ResultadoDiferencia PRIMARY KEY,
        Nombre      VARCHAR(20) NOT NULL CONSTRAINT UQ_ResultadoDiferencia_Nombre UNIQUE
    );
    INSERT INTO dbo.ResultadoDiferencia (IdResultado, Nombre) VALUES
        (1, 'Faltante'), (2, 'FueraDeLugar'), (3, 'Desconocido');
END
GO

IF OBJECT_ID('dbo.ResolucionDiferencia', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ResolucionDiferencia (
        IdResolucion INT         NOT NULL CONSTRAINT PK_ResolucionDiferencia PRIMARY KEY,
        Nombre       VARCHAR(20) NOT NULL CONSTRAINT UQ_ResolucionDiferencia_Nombre UNIQUE
    );
    INSERT INTO dbo.ResolucionDiferencia (IdResolucion, Nombre) VALUES (1, 'Baja'), (2, 'Reubicado');
END
GO

IF OBJECT_ID('dbo.MedioDePago', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.MedioDePago (
        IdMedioDePago INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_MedioDePago PRIMARY KEY,
        Nombre        NVARCHAR(30) NOT NULL CONSTRAINT UQ_MedioDePago_Nombre UNIQUE,
        EsTarjeta     BIT          NOT NULL CONSTRAINT DF_MedioDePago_EsTarjeta DEFAULT (0)
    );
    INSERT INTO dbo.MedioDePago (Nombre, EsTarjeta) VALUES
        (N'Efectivo', 0), (N'Tarjeta de débito', 1), (N'Tarjeta de crédito', 1), (N'Transferencia', 0);
END
GO

IF OBJECT_ID('dbo.Parametro', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Parametro (
        Nombre      VARCHAR(50)   NOT NULL CONSTRAINT PK_Parametro PRIMARY KEY,
        Valor       NVARCHAR(100) NOT NULL,
        Descripcion NVARCHAR(200) NOT NULL CONSTRAINT DF_Parametro_Descripcion DEFAULT ('')
    );
END
GO

MERGE dbo.Parametro AS destino
USING (VALUES
    ('DiasAvisoVencimiento',           N'7',      N'Días antes del vencimiento en que una membresía pasa a "por vencer".'),
    ('MaximoMesesPorPago',             N'12',     N'Cantidad máxima de meses que se pueden pagar en una renovación.'),
    ('MaximoFilasReporteSocios',       N'100000', N'Tope de filas del reporte de socios.'),
    ('MaximoResultadosBusquedaSocios', N'50',     N'Tope de resultados al buscar socios por texto.'),
    ('MaximoFilasBitacoraCambios',     N'2000',   N'Tope de filas de la consulta de la bitácora de cambios.'),
    ('MotivoBajaPorPerdida',           N'Pérdida detectada en auditoría', N'Motivo que se registra al dar de baja un faltante de auditoría.'),
    ('MotivoDeterioroEnAuditoria',     N'Deterioro observado en auditoría', N'Motivo que se registra al marcar como deteriorado un ejemplar durante una auditoría.')
) AS origen (Nombre, Valor, Descripcion)
ON destino.Nombre = origen.Nombre
WHEN NOT MATCHED THEN
    INSERT (Nombre, Valor, Descripcion) VALUES (origen.Nombre, origen.Valor, origen.Descripcion);
GO

/* =====================================================================
   2. Ejemplar: Estado / Origen -> IdEstado / IdOrigen
   ===================================================================== */

IF COL_LENGTH('dbo.Ejemplar', 'IdEstado') IS NULL
    ALTER TABLE dbo.Ejemplar ADD IdEstado INT NULL, IdOrigen INT NULL;
GO

IF COL_LENGTH('dbo.Ejemplar', 'Estado') IS NOT NULL
BEGIN
    EXEC (N'
        UPDATE e SET e.IdEstado = es.IdEstado, e.IdOrigen = o.IdOrigen
        FROM dbo.Ejemplar e
        LEFT JOIN dbo.EstadoEjemplar es ON es.Nombre = e.Estado
        LEFT JOIN dbo.OrigenEjemplar o  ON o.Nombre  = e.Origen;');

    IF EXISTS (SELECT 1 FROM dbo.Ejemplar WHERE IdEstado IS NULL OR IdOrigen IS NULL)
        RAISERROR('Hay ejemplares con un estado u origen que no esta en las tablas nuevas. No se borro nada: revisar antes de seguir.', 16, 1);
    ELSE
    BEGIN
        EXEC #QuitarColumna 'dbo.Ejemplar', 'Estado';
        EXEC #QuitarColumna 'dbo.Ejemplar', 'Origen';
    END
END
GO

IF COL_LENGTH('dbo.Ejemplar', 'Estado') IS NULL
   AND OBJECT_ID('dbo.FK_Ejemplar_EstadoEjemplar', 'F') IS NULL
BEGIN
    ALTER TABLE dbo.Ejemplar ALTER COLUMN IdEstado INT NOT NULL;
    ALTER TABLE dbo.Ejemplar ALTER COLUMN IdOrigen INT NOT NULL;
    ALTER TABLE dbo.Ejemplar ADD
        CONSTRAINT DF_Ejemplar_IdEstado DEFAULT (1) FOR IdEstado,
        CONSTRAINT FK_Ejemplar_EstadoEjemplar FOREIGN KEY (IdEstado) REFERENCES dbo.EstadoEjemplar (IdEstado),
        CONSTRAINT FK_Ejemplar_OrigenEjemplar FOREIGN KEY (IdOrigen) REFERENCES dbo.OrigenEjemplar (IdOrigen);
END
GO

/* El indice por seccion y estado se borro junto con la columna Estado: se rehace con IdEstado. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Ejemplar_Seccion_Estado' AND object_id = OBJECT_ID('dbo.Ejemplar'))
    CREATE INDEX IX_Ejemplar_Seccion_Estado ON dbo.Ejemplar (Seccion, IdEstado);
GO

/* =====================================================================
   3. EjemplarMovimiento: Tipo / EstadoAnterior / EstadoNuevo -> Ids
   ===================================================================== */

IF COL_LENGTH('dbo.EjemplarMovimiento', 'IdTipoMovimiento') IS NULL
    ALTER TABLE dbo.EjemplarMovimiento ADD IdTipoMovimiento INT NULL, IdEstadoAnterior INT NULL, IdEstadoNuevo INT NULL;
GO

IF COL_LENGTH('dbo.EjemplarMovimiento', 'Tipo') IS NOT NULL
BEGIN
    EXEC (N'
        UPDATE m SET m.IdTipoMovimiento = t.IdTipoMovimiento,
                     m.IdEstadoAnterior = ea.IdEstado,
                     m.IdEstadoNuevo    = en.IdEstado
        FROM dbo.EjemplarMovimiento m
        LEFT JOIN dbo.TipoMovimientoEjemplar t ON t.Nombre = m.Tipo
        LEFT JOIN dbo.EstadoEjemplar ea ON ea.Nombre = m.EstadoAnterior
        LEFT JOIN dbo.EstadoEjemplar en ON en.Nombre = m.EstadoNuevo;');

    IF EXISTS (SELECT 1 FROM dbo.EjemplarMovimiento WHERE IdTipoMovimiento IS NULL)
        RAISERROR('Hay movimientos con un tipo que no esta en TipoMovimientoEjemplar. No se borro nada.', 16, 1);
    ELSE
    BEGIN
        EXEC #QuitarColumna 'dbo.EjemplarMovimiento', 'Tipo';
        EXEC #QuitarColumna 'dbo.EjemplarMovimiento', 'EstadoAnterior';
        EXEC #QuitarColumna 'dbo.EjemplarMovimiento', 'EstadoNuevo';
    END
END
GO

IF COL_LENGTH('dbo.EjemplarMovimiento', 'Tipo') IS NULL
   AND OBJECT_ID('dbo.FK_EjemplarMovimiento_Tipo', 'F') IS NULL
BEGIN
    ALTER TABLE dbo.EjemplarMovimiento ALTER COLUMN IdTipoMovimiento INT NOT NULL;
    ALTER TABLE dbo.EjemplarMovimiento ADD
        CONSTRAINT FK_EjemplarMovimiento_Tipo           FOREIGN KEY (IdTipoMovimiento) REFERENCES dbo.TipoMovimientoEjemplar (IdTipoMovimiento),
        CONSTRAINT FK_EjemplarMovimiento_EstadoAnterior FOREIGN KEY (IdEstadoAnterior) REFERENCES dbo.EstadoEjemplar (IdEstado),
        CONSTRAINT FK_EjemplarMovimiento_EstadoNuevo    FOREIGN KEY (IdEstadoNuevo)    REFERENCES dbo.EstadoEjemplar (IdEstado);
END
GO

/* =====================================================================
   4. AuditoriaDiferencia: Resultado / Resolucion -> IdResultado / IdResolucion
   ===================================================================== */

IF COL_LENGTH('dbo.AuditoriaDiferencia', 'IdResultado') IS NULL
    ALTER TABLE dbo.AuditoriaDiferencia ADD IdResultado INT NULL, IdResolucion INT NULL;
GO

IF COL_LENGTH('dbo.AuditoriaDiferencia', 'Resultado') IS NOT NULL
BEGIN
    EXEC (N'
        UPDATE d SET d.IdResultado = r.IdResultado, d.IdResolucion = s.IdResolucion
        FROM dbo.AuditoriaDiferencia d
        LEFT JOIN dbo.ResultadoDiferencia r  ON r.Nombre = d.Resultado
        LEFT JOIN dbo.ResolucionDiferencia s ON s.Nombre = d.Resolucion;');

    IF EXISTS (SELECT 1 FROM dbo.AuditoriaDiferencia WHERE IdResultado IS NULL)
        RAISERROR('Hay diferencias con un resultado que no esta en ResultadoDiferencia. No se borro nada.', 16, 1);
    ELSE
    BEGIN
        EXEC #QuitarColumna 'dbo.AuditoriaDiferencia', 'Resultado';
        EXEC #QuitarColumna 'dbo.AuditoriaDiferencia', 'Resolucion';
    END
END
GO

IF COL_LENGTH('dbo.AuditoriaDiferencia', 'Resultado') IS NULL
   AND OBJECT_ID('dbo.FK_AuditoriaDiferencia_Resultado', 'F') IS NULL
BEGIN
    ALTER TABLE dbo.AuditoriaDiferencia ALTER COLUMN IdResultado INT NOT NULL;
    ALTER TABLE dbo.AuditoriaDiferencia ADD
        CONSTRAINT FK_AuditoriaDiferencia_Resultado  FOREIGN KEY (IdResultado)  REFERENCES dbo.ResultadoDiferencia (IdResultado),
        CONSTRAINT FK_AuditoriaDiferencia_Resolucion FOREIGN KEY (IdResolucion) REFERENCES dbo.ResolucionDiferencia (IdResolucion);
END
GO

/* =====================================================================
   5. Pago: MedioDePago -> IdMedioDePago
   ===================================================================== */

/* Si algun pago viejo tiene un medio que no esta en la tabla, se agrega para no perder el dato. */
IF COL_LENGTH('dbo.Pago', 'MedioDePago') IS NOT NULL
    EXEC (N'
        INSERT INTO dbo.MedioDePago (Nombre, EsTarjeta)
        SELECT DISTINCT p.MedioDePago, CASE WHEN p.MedioDePago LIKE N''Tarjeta%'' THEN 1 ELSE 0 END
        FROM dbo.Pago p
        WHERE p.MedioDePago <> N''''
          AND NOT EXISTS (SELECT 1 FROM dbo.MedioDePago m WHERE m.Nombre = p.MedioDePago);');
GO

IF COL_LENGTH('dbo.Pago', 'IdMedioDePago') IS NULL
    ALTER TABLE dbo.Pago ADD IdMedioDePago INT NULL;
GO

IF COL_LENGTH('dbo.Pago', 'MedioDePago') IS NOT NULL
BEGIN
    EXEC (N'
        UPDATE p SET p.IdMedioDePago = m.IdMedioDePago
        FROM dbo.Pago p
        LEFT JOIN dbo.MedioDePago m ON m.Nombre = p.MedioDePago;');

    IF EXISTS (SELECT 1 FROM dbo.Pago WHERE IdMedioDePago IS NULL)
        RAISERROR('Hay pagos sin medio de pago reconocible. No se borro nada.', 16, 1);
    ELSE
        EXEC #QuitarColumna 'dbo.Pago', 'MedioDePago';
END
GO

IF COL_LENGTH('dbo.Pago', 'MedioDePago') IS NULL
   AND OBJECT_ID('dbo.FK_Pago_MedioDePago', 'F') IS NULL
BEGIN
    ALTER TABLE dbo.Pago ALTER COLUMN IdMedioDePago INT NOT NULL;
    ALTER TABLE dbo.Pago ADD CONSTRAINT FK_Pago_MedioDePago FOREIGN KEY (IdMedioDePago) REFERENCES dbo.MedioDePago (IdMedioDePago);
END
GO

DROP PROCEDURE #QuitarColumna;
GO

/* =====================================================================
   6. Control: filas por tabla y columnas que tienen que haber desaparecido
   ===================================================================== */

SELECT 'EstadoEjemplar' AS Tabla, COUNT(*) AS Filas FROM dbo.EstadoEjemplar
UNION ALL SELECT 'EstadoEjemplarTransicion', COUNT(*) FROM dbo.EstadoEjemplarTransicion
UNION ALL SELECT 'OrigenEjemplar', COUNT(*) FROM dbo.OrigenEjemplar
UNION ALL SELECT 'TipoMovimientoEjemplar', COUNT(*) FROM dbo.TipoMovimientoEjemplar
UNION ALL SELECT 'ResultadoDiferencia', COUNT(*) FROM dbo.ResultadoDiferencia
UNION ALL SELECT 'ResolucionDiferencia', COUNT(*) FROM dbo.ResolucionDiferencia
UNION ALL SELECT 'MedioDePago', COUNT(*) FROM dbo.MedioDePago
UNION ALL SELECT 'Parametro', COUNT(*) FROM dbo.Parametro
UNION ALL SELECT 'Ejemplar', COUNT(*) FROM dbo.Ejemplar
UNION ALL SELECT 'EjemplarMovimiento', COUNT(*) FROM dbo.EjemplarMovimiento
UNION ALL SELECT 'AuditoriaDiferencia', COUNT(*) FROM dbo.AuditoriaDiferencia
UNION ALL SELECT 'Pago', COUNT(*) FROM dbo.Pago;

SELECT t.name AS Tabla, c.name AS ColumnaVieja_NoDeberiaAparecer
FROM sys.columns c
JOIN sys.tables t ON t.object_id = c.object_id
WHERE (t.name = 'Ejemplar' AND c.name IN ('Estado', 'Origen'))
   OR (t.name = 'EjemplarMovimiento' AND c.name IN ('Tipo', 'EstadoAnterior', 'EstadoNuevo'))
   OR (t.name = 'AuditoriaDiferencia' AND c.name IN ('Resultado', 'Resolucion'))
   OR (t.name = 'Pago' AND c.name = 'MedioDePago');
GO
