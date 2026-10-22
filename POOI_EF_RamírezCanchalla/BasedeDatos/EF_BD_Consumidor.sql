IF DB_ID('ExamenFinal') IS NULL
    CREATE DATABASE ExamenFinal;
GO

USE ExamenFinal;
GO
--
IF OBJECT_ID('tb_Consumidor', 'U') IS NOT NULL DROP TABLE tb_Consumidor;
GO
IF OBJECT_ID('tb_paises', 'U') IS NOT NULL DROP TABLE tb_paises;
GO

CREATE TABLE tb_paises (
    idpais  CHAR(3)     NOT NULL,
    nompais VARCHAR(60) NOT NULL,
    CONSTRAINT PK_tb_paises PRIMARY KEY (idpais));
GO
--
INSERT INTO tb_paises (idpais, nompais) VALUES
 ('PER', 'Peru'),
 ('ARG', 'Argentina'),
 ('CHL', 'Chile'),
 ('COL', 'Colombia'),
 ('MEX', 'Mexico'),
 ('ESP', 'Espana'),
 ('USA', 'Estados Unidos'),
 ('BRA', 'Brasil');
GO
--
CREATE TABLE tb_Consumidor (
    dnicont   CHAR(8)      NOT NULL,
    nomcont   VARCHAR(40)  NOT NULL,
    apecont   VARCHAR(40)  NOT NULL,
    emailcont VARCHAR(250) NULL,
    idpais    CHAR(3)      NULL,
    CONSTRAINT PK_tb_Consumidor       PRIMARY KEY (dnicont),
    CONSTRAINT UQ_tb_Consumidor_email UNIQUE (emailcont),
    CONSTRAINT FK_tb_Consumidor_pais  FOREIGN KEY (idpais) REFERENCES tb_paises(idpais));
GO
--
CREATE OR ALTER PROCEDURE usp_agrega_consumidor
    @dnicont   CHAR(8),
    @nomcont   VARCHAR(40),
    @apecont   VARCHAR(40),
    @emailcont VARCHAR(250),
    @idpais    CHAR(3),
    @resultado VARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
--
    IF (@dnicont IS NULL OR LEN(@dnicont) <> 8 OR @dnicont LIKE '%[^0-9]%')
    BEGIN
        SET @resultado = 'El DNI debe ser numerico y de 8 digitos.';
        RETURN;
    END
--
    IF (@nomcont IS NULL OR LTRIM(RTRIM(@nomcont)) = '')
    BEGIN
        SET @resultado = 'El nombre no debe estar vacio.';
        RETURN;
    END
--
    IF (@apecont IS NULL OR LTRIM(RTRIM(@apecont)) = '')
    BEGIN
        SET @resultado = 'El apellido no debe estar vacio.';
        RETURN;
    END
--
    IF EXISTS (SELECT 1 FROM tb_Consumidor WHERE dnicont = @dnicont)
    BEGIN
        SET @resultado = 'Ya existe un consumidor con ese DNI.';
        RETURN;
    END
--
    INSERT INTO tb_Consumidor (dnicont, nomcont, apecont, emailcont, idpais)
    VALUES (@dnicont, @nomcont, @apecont, @emailcont, @idpais);
    SET @resultado = 'Consumidor agregado correctamente.';
END
GO
--
CREATE OR ALTER PROCEDURE usp_eliminar_consumidor
    @dnicont CHAR(8)
AS
BEGIN
    SET NOCOUNT ON;

--
    IF NOT EXISTS (SELECT 1 FROM tb_Consumidor WHERE dnicont = @dnicont)
        RETURN;
    DELETE FROM tb_Consumidor WHERE dnicont = @dnicont;
END
GO
--
CREATE OR ALTER PROCEDURE usp_listar_consumidor
AS
BEGIN
    SET NOCOUNT ON;
    SELECT dnicont, nomcont, apecont, emailcont, idpais
    FROM tb_Consumidor;
END
GO
--
CREATE OR ALTER PROCEDURE usp_obtener_consumidor
    @dnicont CHAR(8)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT dnicont, nomcont, apecont, emailcont, idpais
    FROM tb_Consumidor
    WHERE dnicont = @dnicont;
END
GO
--
CREATE OR ALTER PROCEDURE usp_actualizar_consumidor
    @dnicont   CHAR(8),
    @nomcont   VARCHAR(40),
    @apecont   VARCHAR(40),
    @emailcont VARCHAR(250),
    @idpais    CHAR(3)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tb_Consumidor
SET nomcont   = @nomcont,
    apecont   = @apecont,
    emailcont = @emailcont,
    idpais    = @idpais
WHERE dnicont = @dnicont;
END
GO
--
DECLARE @msg VARCHAR(200);
EXEC usp_agrega_consumidor '12345678','AARON','RAMIREZ','AARON@correo.com','PER', @msg OUTPUT;
PRINT @msg;
--
EXEC usp_listar_consumidor;
EXEC usp_obtener_consumidor '12345678';
EXEC usp_eliminar_consumidor '12345678';
