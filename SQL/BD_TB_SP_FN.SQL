USE [BD_CA_CL]
GO
/****** Object:  User [User_BD_CA_CL]    Script Date: 08-02-2026 23:26:51 ******/
CREATE USER [User_BD_CA_CL] FOR LOGIN [User_BD_CA_CL] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Schema [app]    Script Date: 08-02-2026 23:26:51 ******/
CREATE SCHEMA [app]
GO
/****** Object:  UserDefinedFunction [app].[fn_HashPassword]    Script Date: 08-02-2026 23:26:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   FUNCTION [app].[fn_HashPassword]
(
    @Password NVARCHAR(255)
)
RETURNS NVARCHAR(255)
AS
BEGIN
    DECLARE @Hash VARBINARY(32);

    -- Hash SHA2_256
    SET @Hash = HASHBYTES(
        'SHA2_256',
        CONVERT(VARBINARY(255), @Password)
    );

    -- Retornamos en HEX
    RETURN CONVERT(NVARCHAR(255), @Hash, 2);
END;
GO
/****** Object:  Table [app].[HistorialLogin]    Script Date: 08-02-2026 23:26:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [app].[HistorialLogin](
	[HistorialLoginId] [uniqueidentifier] NOT NULL,
	[UsuarioId] [uniqueidentifier] NULL,
	[IpOrigen] [nvarchar](45) NULL,
	[FechaLogin] [datetime2](7) NOT NULL,
	[Observacion] [nvarchar](255) NULL,
	[ObservacionLoginId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_HistorialLogin] PRIMARY KEY CLUSTERED 
(
	[HistorialLoginId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [app].[Menu]    Script Date: 08-02-2026 23:26:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [app].[Menu](
	[MenuId] [uniqueidentifier] NOT NULL,
	[MenuPadreId] [uniqueidentifier] NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Icono] [nvarchar](50) NULL,
	[Ruta] [nvarchar](200) NULL,
	[Orden] [int] NOT NULL,
	[EsActivo] [bit] NOT NULL,
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [app].[ObservacionLogin]    Script Date: 08-02-2026 23:26:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [app].[ObservacionLogin](
	[ObservacionLoginId] [uniqueidentifier] NOT NULL,
	[Codigo] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](255) NOT NULL,
	[EsError] [bit] NOT NULL,
	[EsActivo] [bit] NOT NULL,
	[FechaCreacion] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ObservacionLogin] PRIMARY KEY CLUSTERED 
(
	[ObservacionLoginId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_ObservacionLogin_Codigo] UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [app].[Rol]    Script Date: 08-02-2026 23:26:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [app].[Rol](
	[RolId] [uniqueidentifier] NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](255) NULL,
	[EsActivo] [bit] NOT NULL,
	[FechaCreacion] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED 
(
	[RolId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Rol_Nombre] UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [app].[RolMenu]    Script Date: 08-02-2026 23:26:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [app].[RolMenu](
	[RolId] [uniqueidentifier] NOT NULL,
	[MenuId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_RolMenu] PRIMARY KEY CLUSTERED 
(
	[RolId] ASC,
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [app].[Usuario]    Script Date: 08-02-2026 23:26:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [app].[Usuario](
	[UsuarioId] [uniqueidentifier] NOT NULL,
	[Usuario] [nvarchar](50) NOT NULL,
	[Clave] [nvarchar](255) NOT NULL,
	[Nombre] [nvarchar](150) NOT NULL,
	[Email] [nvarchar](150) NULL,
	[RolId] [uniqueidentifier] NOT NULL,
	[EsActivo] [bit] NOT NULL,
	[FechaCreacion] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[UsuarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Usuario_Usuario] UNIQUE NONCLUSTERED 
(
	[Usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [app].[HistorialLogin] ADD  DEFAULT (newsequentialid()) FOR [HistorialLoginId]
GO
ALTER TABLE [app].[HistorialLogin] ADD  DEFAULT (sysdatetime()) FOR [FechaLogin]
GO
ALTER TABLE [app].[Menu] ADD  DEFAULT (newsequentialid()) FOR [MenuId]
GO
ALTER TABLE [app].[Menu] ADD  DEFAULT ((1)) FOR [EsActivo]
GO
ALTER TABLE [app].[ObservacionLogin] ADD  DEFAULT (newsequentialid()) FOR [ObservacionLoginId]
GO
ALTER TABLE [app].[ObservacionLogin] ADD  DEFAULT ((1)) FOR [EsActivo]
GO
ALTER TABLE [app].[ObservacionLogin] ADD  DEFAULT (sysdatetime()) FOR [FechaCreacion]
GO
ALTER TABLE [app].[Rol] ADD  DEFAULT (newsequentialid()) FOR [RolId]
GO
ALTER TABLE [app].[Rol] ADD  DEFAULT ((1)) FOR [EsActivo]
GO
ALTER TABLE [app].[Rol] ADD  DEFAULT (sysdatetime()) FOR [FechaCreacion]
GO
ALTER TABLE [app].[Usuario] ADD  DEFAULT (newsequentialid()) FOR [UsuarioId]
GO
ALTER TABLE [app].[Usuario] ADD  DEFAULT ((1)) FOR [EsActivo]
GO
ALTER TABLE [app].[Usuario] ADD  DEFAULT (sysdatetime()) FOR [FechaCreacion]
GO
ALTER TABLE [app].[HistorialLogin]  WITH CHECK ADD  CONSTRAINT [FK_HistorialLogin_Observacion] FOREIGN KEY([ObservacionLoginId])
REFERENCES [app].[ObservacionLogin] ([ObservacionLoginId])
GO
ALTER TABLE [app].[HistorialLogin] CHECK CONSTRAINT [FK_HistorialLogin_Observacion]
GO
ALTER TABLE [app].[HistorialLogin]  WITH CHECK ADD  CONSTRAINT [FK_HistorialLogin_Usuario] FOREIGN KEY([UsuarioId])
REFERENCES [app].[Usuario] ([UsuarioId])
GO
ALTER TABLE [app].[HistorialLogin] CHECK CONSTRAINT [FK_HistorialLogin_Usuario]
GO
ALTER TABLE [app].[Menu]  WITH CHECK ADD  CONSTRAINT [FK_Menu_Padre] FOREIGN KEY([MenuPadreId])
REFERENCES [app].[Menu] ([MenuId])
GO
ALTER TABLE [app].[Menu] CHECK CONSTRAINT [FK_Menu_Padre]
GO
ALTER TABLE [app].[RolMenu]  WITH NOCHECK ADD  CONSTRAINT [FK_RolMenu_Menu] FOREIGN KEY([MenuId])
REFERENCES [app].[Menu] ([MenuId])
GO
ALTER TABLE [app].[RolMenu] CHECK CONSTRAINT [FK_RolMenu_Menu]
GO
ALTER TABLE [app].[RolMenu]  WITH CHECK ADD  CONSTRAINT [FK_RolMenu_Rol] FOREIGN KEY([RolId])
REFERENCES [app].[Rol] ([RolId])
GO
ALTER TABLE [app].[RolMenu] CHECK CONSTRAINT [FK_RolMenu_Rol]
GO
ALTER TABLE [app].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Rol] FOREIGN KEY([RolId])
REFERENCES [app].[Rol] ([RolId])
GO
ALTER TABLE [app].[Usuario] CHECK CONSTRAINT [FK_Usuario_Rol]
GO
/****** Object:  StoredProcedure [app].[sp_CrearMenuHijo]    Script Date: 08-02-2026 23:26:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [app].[sp_CrearMenuHijo]
    @MenuPadreId UNIQUEIDENTIFIER,
    @Nombre NVARCHAR(100),
    @Ruta NVARCHAR(200),
    @Orden INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ExisteNombre NVARCHAR(100);
    DECLARE @MenuId UNIQUEIDENTIFIER;

    -- Verificar si ya existe un menú hijo con ese orden dentro del mismo padre
    SELECT @ExisteNombre = Nombre
    FROM [app].[Menu]
    WHERE MenuPadreId = @MenuPadreId
      AND Orden = @Orden;

    IF @ExisteNombre IS NOT NULL
    BEGIN
        SELECT 
            Mensaje = CONCAT('Ya existe un número de orden para este menú padre. El nombre que ocupa ese orden es: ', @ExisteNombre),
            MenuId = NULL;
        RETURN;
    END

    -- Verificar si ya existe un menú hijo con el mismo nombre dentro del mismo padre
    SELECT @ExisteNombre = Nombre
    FROM [app].[Menu]
    WHERE MenuPadreId = @MenuPadreId
      AND Nombre = @Nombre;

    IF @ExisteNombre IS NOT NULL
    BEGIN
        SELECT 
            Mensaje = CONCAT('Ya existe un menú hijo con el nombre: ', @ExisteNombre, ' bajo este menú padre.'),
            MenuId = NULL;
        RETURN;
    END

    -- Crear nuevo MenuId
    SET @MenuId = NEWID();

    -- Insertar nuevo menú hijo (Icono siempre NULL)
    INSERT INTO [app].[Menu] (
        MenuId,
        MenuPadreId,
        Nombre,
        Icono,
        Ruta,
        Orden,
        EsActivo
    )
    VALUES (
        @MenuId,
        @MenuPadreId,
        @Nombre,
        NULL,          -- Icono fijo en NULL
        @Ruta,
        @Orden,
        1
    );

    -- Retornar mensaje de éxito y el MenuId creado
    SELECT 
        Mensaje = 'Se creó el menú hijo con éxito.',
        MenuId = @MenuId;
END;
GO
/****** Object:  StoredProcedure [app].[sp_CrearMenuPadre]    Script Date: 08-02-2026 23:26:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [app].[sp_CrearMenuPadre]
    @Nombre NVARCHAR(100),
    @Icono NVARCHAR(50),
    @Ruta NVARCHAR(200),
    @Orden INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ExisteNombre NVARCHAR(100);
    DECLARE @MenuId UNIQUEIDENTIFIER;

    -- Verificar si ya existe un menú padre con ese orden
    SELECT @ExisteNombre = Nombre
    FROM [app].[Menu]
    WHERE MenuPadreId IS NULL
      AND Orden = @Orden;

    IF @ExisteNombre IS NOT NULL
    BEGIN
        SELECT 
            Mensaje = CONCAT('Ya existe un número de orden para el menú padre. El nombre que ocupa ese orden es: ', @ExisteNombre),
            MenuId = NULL;
        RETURN;
    END

    -- Verificar si ya existe un menú padre con ese nombre
    SELECT @ExisteNombre = Nombre
    FROM [app].[Menu]
    WHERE MenuPadreId IS NULL
      AND Nombre = @Nombre;

    IF @ExisteNombre IS NOT NULL
    BEGIN
        SELECT 
            Mensaje = CONCAT('Ya existe un menú padre con el nombre: ', @ExisteNombre),
            MenuId = NULL;
        RETURN;
    END

    -- Crear nuevo MenuId
    SET @MenuId = NEWID();

    -- Insertar nuevo menú padre
    INSERT INTO [app].[Menu] (
        MenuId,
        MenuPadreId,
        Nombre,
        Icono,
        Ruta,
        Orden,
        EsActivo
    )
    VALUES (
        @MenuId,
        NULL,
        @Nombre,
        @Icono,
        @Ruta,
        @Orden,
        1
    );

    -- Retornar mensaje de éxito y el MenuId creado
    SELECT 
        Mensaje = 'Se creó el menú padre con éxito.',
        MenuId = @MenuId;
END;
GO
/****** Object:  StoredProcedure [app].[sp_CrearMenuRol]    Script Date: 08-02-2026 23:26:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [app].[sp_CrearMenuRol]
    @RolId UNIQUEIDENTIFIER,
    @MenuId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Existe INT;

    -- Validar si ya existe la relación RolId - MenuId
    SELECT @Existe = COUNT(*)
    FROM [app].[RolMenu]
    WHERE RolId = @RolId
      AND MenuId = @MenuId;

    IF @Existe > 0
    BEGIN
        SELECT 
            Mensaje = CONCAT('Ya existe el registro con RolId: ', CAST(@RolId AS NVARCHAR(36)), 
                             ' y MenuId: ', CAST(@MenuId AS NVARCHAR(36)));
        RETURN;
    END

    -- Insertar nuevo registro
    INSERT INTO [app].[RolMenu] (
        RolId,
        MenuId
    )
    VALUES (
        @RolId,
        @MenuId
    );

    -- Retornar mensaje de éxito
    SELECT 
        Mensaje = 'Se creó el registro Rol-Menu con éxito.';
END;
GO
/****** Object:  StoredProcedure [app].[sp_LoginUsuario]    Script Date: 08-02-2026 23:26:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [app].[sp_LoginUsuario]
(
    @Email     NVARCHAR(100),
    @Clave     NVARCHAR(200),
    @IpOrigen  NVARCHAR(45)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE
        @UsuarioId UNIQUEIDENTIFIER = NULL,
        @RolId UNIQUEIDENTIFIER = NULL,
        @Nombre NVARCHAR(150) = NULL,
        @ObservacionLoginId UNIQUEIDENTIFIER,
        @Codigo NVARCHAR(50),
        @Descripcion NVARCHAR(255),
        @ClaveHash NVARCHAR(255),
		@RolNombre NVARCHAR(100);

    -- Hash de la clave
    SET @ClaveHash = app.fn_HashPassword(@Clave);

    -- Buscar usuario
    SELECT
        @UsuarioId = u.UsuarioId,
        @RolId = u.RolId,
        @Nombre = u.Nombre,
		@RolNombre = r.Nombre
    FROM app.Usuario u
	LEFT JOIN app.Rol r on u.RolId = r.RolId
    WHERE u.Email = @Email
      AND u.EsActivo = 1;

    /* ==========================
       USUARIO NO EXISTE
       ========================== */
    IF @UsuarioId IS NULL
    BEGIN
        SELECT
            @ObservacionLoginId = ol.ObservacionLoginId,
            @Codigo = ol.Codigo,
            @Descripcion = ol.Descripcion
        FROM app.ObservacionLogin ol
        WHERE ol.Codigo = 'USER_NOT_FOUND'
          AND ol.EsActivo = 1;

        INSERT INTO app.HistorialLogin
        (
            UsuarioId,
            IpOrigen,
            Observacion,
            ObservacionLoginId
        )
        VALUES
        (
            NULL,
            @IpOrigen,
            @Descripcion,
            @ObservacionLoginId
        );

        SELECT
            @Codigo AS Codigo,
            @Descripcion AS Descripcion,
            NULL AS UsuarioId,
            NULL AS RolId,
            NULL AS Nombre,
			NULL AS Email,
			NULL AS RolNombre;

        RETURN;
    END

    /* ==========================
       CLAVE INCORRECTA
       ========================== */
    IF NOT EXISTS
    (
        SELECT 1
        FROM app.Usuario
        WHERE UsuarioId = @UsuarioId
          AND Clave = @ClaveHash
    )
    BEGIN
        SELECT
            @ObservacionLoginId = ol.ObservacionLoginId,
            @Codigo = ol.Codigo,
            @Descripcion = ol.Descripcion
        FROM app.ObservacionLogin ol
        WHERE ol.Codigo = 'INVALID_PASSWORD'
          AND ol.EsActivo = 1;

        INSERT INTO app.HistorialLogin
        (
            UsuarioId,
            IpOrigen,
            Observacion,
            ObservacionLoginId
        )
        VALUES
        (
            @UsuarioId,
            @IpOrigen,
            @Descripcion,
            @ObservacionLoginId
        );

        SELECT
            @Codigo AS Codigo,
            @Descripcion AS Descripcion,
			NULL AS UsuarioId,
			NULL AS RolId,
			NULL AS Nombre,
			NULL AS Email,
			NULL AS RolNombre;

        RETURN;
    END

    /* ==========================
       LOGIN OK
       ========================== */
    SELECT
        @ObservacionLoginId = ol.ObservacionLoginId,
        @Codigo = ol.Codigo,
        @Descripcion = ol.Descripcion
    FROM app.ObservacionLogin ol
    WHERE ol.Codigo = 'LOGIN_OK'
      AND ol.EsActivo = 1;

    INSERT INTO app.HistorialLogin
    (
        UsuarioId,
        IpOrigen,
        Observacion,
        ObservacionLoginId
    )
    VALUES
    (
        @UsuarioId,
        @IpOrigen,
        @Descripcion,
        @ObservacionLoginId
    );

    SELECT
        @Codigo AS Codigo,
        @Descripcion AS Descripcion,
        @UsuarioId AS UsuarioId,
        @RolId AS RolId,
        @Nombre AS Nombre,
		@Email AS Email,
		@RolNombre as RolNombre;
END
GO
/****** Object:  StoredProcedure [app].[sp_MenuPorRol]    Script Date: 08-02-2026 23:26:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [app].[sp_MenuPorRol]
    @RolId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        m.MenuId,
        m.MenuPadreId,
        m.Nombre,
        m.Icono,
        m.Ruta,
        m.Orden
    FROM app.RolMenu rm
    INNER JOIN app.Menu m ON m.MenuId = rm.MenuId
    WHERE rm.RolId = @RolId
      AND m.EsActivo = 1
    ORDER BY 
        ISNULL(m.MenuPadreId, m.MenuId),
        m.Orden;
END;
GO
/****** Object:  StoredProcedure [app].[sp_RegistrarUsuario]    Script Date: 08-02-2026 23:26:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [app].[sp_RegistrarUsuario]
(
    @Usuario NVARCHAR(50),
    @Clave   NVARCHAR(200),
    @Nombre  NVARCHAR(150),
    @Email   NVARCHAR(150)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE
        @ClaveHash NVARCHAR(255),
        @RolId UNIQUEIDENTIFIER = '56F158D6-9B04-F111-851D-0EA34A736CA1',
        @Codigo NVARCHAR(50),
        @Descripcion NVARCHAR(255);

    /* ==========================
       VALIDAR USUARIO EXISTENTE
       ========================== */
    IF EXISTS (
        SELECT 1
        FROM app.Usuario
        WHERE Usuario = @Usuario
    )
    BEGIN
        SELECT
            'USER_ALREADY_EXISTS' AS Codigo,
            'El nombre de usuario ya existe.' AS Descripcion;

        RETURN;
    END

    /* ==========================
       VALIDAR EMAIL EXISTENTE
       ========================== */
    IF EXISTS (
        SELECT 1
        FROM app.Usuario
        WHERE Email = @Email
    )
    BEGIN
        SELECT
            'EMAIL_ALREADY_EXISTS' AS Codigo,
            'El correo electrónico ya se encuentra registrado.' AS Descripcion;

        RETURN;
    END

    /* ==========================
       REGISTRO DE USUARIO
       ========================== */
    SET @ClaveHash = app.fn_HashPassword(@Clave);

    INSERT INTO app.Usuario
    (
        Usuario,
        Clave,
        Nombre,
        Email,
        RolId,
        EsActivo,
        FechaCreacion
    )
    VALUES
    (
        @Usuario,
        @ClaveHash,
        @Nombre,
        @Email,
        @RolId,
        1,
        SYSDATETIME()
    );

    SELECT
        'USER_CREATED' AS Codigo,
        'Usuario registrado correctamente.' AS Descripcion;
END
GO
