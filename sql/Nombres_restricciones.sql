/*
    Alejandria_DB - Nombres de restricciones
    Las tablas Socio, Membresia, Suscripcion y Pago se crearon con restricciones sin nombre,
    y SQL Server les asignó nombres automáticos (PK__Socio__863F..., FK__Pago__IdSocio__6EF5...).
    Este script les pone el nombre que figura en el DER (G07), con el mismo criterio que el resto
    de las tablas: PK_Tabla, FK_Tabla_Referenciada, UQ_Tabla_Columna, DF_Tabla_Columna, CK_Tabla_Columna.
    Es idempotente: si la restricción ya tiene el nombre correcto no hace nada.
*/
USE Alejandria_DB;
GO
SET NOCOUNT ON;

DECLARE @mapa TABLE (tipo CHAR(2), tabla SYSNAME, columna SYSNAME, nuevo SYSNAME);
INSERT @mapa (tipo, tabla, columna, nuevo) VALUES
    ('PK', 'Membresia',   'IdMembresia',   'PK_Membresia'),
    ('UQ', 'Membresia',   'Nombre',        'UQ_Membresia_Nombre'),
    ('D',  'Membresia',   'Activa',        'DF_Membresia_Activa'),
    ('PK', 'Socio',       'IdSocio',       'PK_Socio'),
    ('UQ', 'Socio',       'DNI',           'UQ_Socio_DNI'),
    ('D',  'Socio',       'Activo',        'DF_Socio_Activo'),
    ('PK', 'Suscripcion', 'IdSuscripcion', 'PK_Suscripcion'),
    ('F',  'Suscripcion', 'IdSocio',       'FK_Suscripcion_Socio'),
    ('F',  'Suscripcion', 'IdMembresia',   'FK_Suscripcion_Membresia'),
    ('D',  'Suscripcion', 'Estado',        'DF_Suscripcion_Estado'),
    ('C',  'Suscripcion', 'Estado',        'CK_Suscripcion_Estado'),
    ('PK', 'Pago',        'IdPago',        'PK_Pago'),
    ('F',  'Pago',        'IdSocio',       'FK_Pago_Socio'),
    ('F',  'Pago',        'IdMembresia',   'FK_Pago_Membresia'),
    ('D',  'Pago',        'FechaPago',     'DF_Pago_FechaPago');

DECLARE @actual SYSNAME, @nuevo SYSNAME, @tabla SYSNAME, @sql NVARCHAR(400);

DECLARE c CURSOR LOCAL FAST_FORWARD FOR
    SELECT m.tabla, m.nuevo,
           CASE m.tipo
             WHEN 'PK' THEN (SELECT TOP 1 k.name FROM sys.key_constraints k
                               JOIN sys.index_columns ic ON ic.object_id = k.parent_object_id AND ic.index_id = k.unique_index_id
                              WHERE k.type = 'PK' AND k.parent_object_id = OBJECT_ID('dbo.' + m.tabla)
                                AND COL_NAME(ic.object_id, ic.column_id) = m.columna)
             WHEN 'UQ' THEN (SELECT TOP 1 k.name FROM sys.key_constraints k
                               JOIN sys.index_columns ic ON ic.object_id = k.parent_object_id AND ic.index_id = k.unique_index_id
                              WHERE k.type = 'UQ' AND k.parent_object_id = OBJECT_ID('dbo.' + m.tabla)
                                AND COL_NAME(ic.object_id, ic.column_id) = m.columna)
             WHEN 'F'  THEN (SELECT TOP 1 fk.name FROM sys.foreign_keys fk
                               JOIN sys.foreign_key_columns fc ON fc.constraint_object_id = fk.object_id
                              WHERE fk.parent_object_id = OBJECT_ID('dbo.' + m.tabla)
                                AND COL_NAME(fc.parent_object_id, fc.parent_column_id) = m.columna)
             WHEN 'D'  THEN (SELECT TOP 1 d.name FROM sys.default_constraints d
                              WHERE d.parent_object_id = OBJECT_ID('dbo.' + m.tabla)
                                AND COL_NAME(d.parent_object_id, d.parent_column_id) = m.columna)
             WHEN 'C'  THEN (SELECT TOP 1 ck.name FROM sys.check_constraints ck
                              WHERE ck.parent_object_id = OBJECT_ID('dbo.' + m.tabla)
                                AND COL_NAME(ck.parent_object_id, ck.parent_column_id) = m.columna)
           END
      FROM @mapa m;

OPEN c;
FETCH NEXT FROM c INTO @tabla, @nuevo, @actual;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF @actual IS NOT NULL AND @actual <> @nuevo
    BEGIN
        SET @sql = N'dbo.' + QUOTENAME(@actual);
        EXEC sp_rename @objname = @sql, @newname = @nuevo, @objtype = 'OBJECT';
        PRINT @tabla + N': ' + @actual + N' -> ' + @nuevo;
    END
    FETCH NEXT FROM c INTO @tabla, @nuevo, @actual;
END
CLOSE c;
DEALLOCATE c;
GO
