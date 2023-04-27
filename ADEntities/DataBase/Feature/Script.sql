--ADD PrecioCompra IN TABLE tRespaldoDiarioInventario
ALTER TABLE tRespaldoDiarioInventario ADD PrecioCompra DECIMAL(10, 2)

--ADD PrecioVenta IN TABLE tRespaldoDiarioInventario
ALTER TABLE tRespaldoDiarioInventario ADD PrecioVenta DECIMAL(10, 2)

--ADD TipoMoneda IN TABLE tRespaldoDiarioInventario
ALTER TABLE tOrden ADD TipoMoneda INT NOT NULL DEFAULT 1

--CREATE TABLE tMoneda
CREATE TABLE [admDB_saadusr].[tMoneda](
	[idMoneda] [int] IDENTITY(1,1) NOT NULL,
	[clave] [varchar](20) NULL,
	[descripcion] [varchar](50) NULL
) ON [PRIMARY]

-- INSERT TABLE tMoneda
INSERT INTO [admDB_saadusr].[tMoneda]
           ([clave]
           ,[descripcion])
     VALUES
           ('MXN'
           ,'Pesos')

INSERT INTO [admDB_saadusr].[tMoneda]
           ([clave]
           ,[descripcion])
     VALUES
           ('USD'
           ,'Dólares')
