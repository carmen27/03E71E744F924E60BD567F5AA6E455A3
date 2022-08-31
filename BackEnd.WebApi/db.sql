DROP TABLE IF EXISTS [dbo].[tproducto];
DROP TABLE IF EXISTS [dbo].[tusuario];
DROP TABLE IF EXISTS [dbo].[tcompradet];
DROP TABLE IF EXISTS [dbo].[tcompra];

CREATE TABLE [dbo].[tcompra](
	[nid] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[cguid] [char](32) NOT NULL,
	[ccodigo] [varchar](10) NOT NULL,
	[ctipo] [char](2) NOT NULL,
	[dfecha] [date] NOT NULL,
	[ccliruc] [varchar](11) NOT NULL,
	[cclirazon] [varchar](100) NULL,
	[nvaligv] [decimal](5, 2) NOT NULL,
	[ntotaligv] [decimal](12, 2) NOT NULL,
	[nimport] [decimal](12, 2) NOT NULL,
	[nimportigv] [decimal](12, 2) NOT NULL,
	[cmoneda] [varchar](5) NOT NULL,
	[cobserv] [varchar](200) NULL,
	[cestado] [char](1) NOT NULL,
	[cusucrea] [varchar](10) NOT NULL,
	[dfeccrea] [datetime] NOT NULL,
	[cusumodi] [varchar](10) NULL,
	[dfecmodi] [datetime] NULL
);

CREATE TABLE [dbo].[tcompradet](
	[nid] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[cprodcod] [varchar](10) NOT NULL,
	[cproddesc] [varchar](100) NULL,
	[cprodmarca] [varchar](10) NULL,
	[cprodunid] [char](3) NULL,
	[nprecio] [decimal](12, 2) NOT NULL,
	[ncantidad] [decimal](12, 2) NOT NULL,
	[nimport] [decimal](12, 2) NOT NULL,
	[cestado] [char](1) NOT NULL,
	[ncompraid] [int] NOT NULL,
	[cusucrea] [varchar](10) NOT NULL,
	[dfeccrea] [datetime] NOT NULL,
	[cusumodi] [varchar](10) NULL,
	[dfecmodi] [datetime] NULL
);

ALTER TABLE [dbo].[tcompradet] ADD CONSTRAINT [fk_tcompradet_0] FOREIGN KEY([ncompraid])
REFERENCES [dbo].[tcompra] ([nid]);

CREATE TABLE [dbo].[tproducto](
	[nid] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[cguid] [char](32) NOT NULL,
	[ccodigo] [varchar](10) NOT NULL,
	[cdescripcion] [varchar](100) NULL,
	[cmarca] [varchar](10) NULL,
	[cunidades] [char](3) NULL,
	[nprecio] [decimal](12, 2) NOT NULL,
	[cestado] [char](1) NOT NULL,
	[cusucrea] [varchar](10) NOT NULL,
	[dfeccrea] [datetime] NOT NULL,
	[cusumodi] [varchar](10) NULL,
	[dfecmodi] [datetime] NULL
);

CREATE TABLE [dbo].[tusuario](
	[nid] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[cguid] [char](32) NOT NULL,
	[ccodigo] [varchar](10) NOT NULL,
	[cnombres] [varchar](50) NOT NULL,
	[capellidos] [varchar](50) NOT NULL,
	[cusername] [varchar](50) NOT NULL,
	[ypassword] [varbinary](max) NOT NULL,
	[ntipdocum] [int] NOT NULL,
	[cnumdocum] [varchar](15) NOT NULL,
	[cemail] [varchar](50) NOT NULL,
	[cnumero1] [varchar](15) NULL,
	[cnumero2] [varchar](15) NULL,
	[cnumero3] [varchar](15) NULL,
	[cestado] [char](1) NOT NULL,
	[cusucrea] [varchar](10) NOT NULL,
	[dfeccrea] [datetime] NOT NULL,
	[cusumodi] [varchar](10) NULL,
	[dfecmodi] [datetime] NULL
);