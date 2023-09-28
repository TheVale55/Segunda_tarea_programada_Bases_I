DECLARE @xmlData XML; -- @XMLData es un variable tipo texto que contiene un documento XML

-- Funciona en una direccion fisica, del archivo que contiene el XML, en la compu donde esta el servidor.
-- Si el servidor esta en la nube, hay una manera de indicar al servidor que busque en una direccion local.
-- Para efectos de pruebas, pueden iniciar asignando el documento XML: Set @xmlData='<DatosPrueba> <TipoDocId>..</TipoDocId>.. </DatosPrueba>'
SET @xmlData = (
		SELECT *
		FROM OPENROWSET(BULK 'C:\Users\HP-PC\Desktop\Datos.xml', SINGLE_BLOB) --Ruta donde se encuentra el archivo con los datos
		AS xmlData
		);

INSERT INTO dbo.Usuario(UserName, Password) --Se inserta todos los usuarios
SELECT  
	T.Item.value('@Nombre', 'VARCHAR(16)'), --Se obtiene el dato nombre del xml
	T.Item.value('@Password', 'VARCHAR(16)') --Se obtiene el dato contraseña del xml
FROM @xmlData.nodes('root/Usuarios/usuario') as T(Item)

INSERT INTO dbo.ClaseArticulo(Nombre) --Se insertan las clases de artículo
SELECT  
	T.Item.value('@Nombre', 'VARCHAR(64)') --Se obtienen los nombres
FROM @xmlData.nodes('root/ClasesdeArticulos/ClasedeArticulos') as T(Item)

INSERT INTO dbo.Articulo(IdClaseArticulo, Codigo, Nombre, Precio, EsActivo) --Se insertan los artículos
SELECT  
	(SELECT A.id FROM ClaseArticulo A WHERE A.Nombre=T.Item.value('@ClasedeArticulos', 'VARCHAR(64)')) --Se obtiene el dato id de clase de articulo
	, T.Item.value('@Codigo', 'VARCHAR(32)') --Se obtiene el dato codigo
	, T.Item.value('@Nombre', 'VARCHAR(128)') --Se obtiene el dato nombre
	, T.Item.value('@Precio', 'MONEY') --Se obtiene el dato precio
	, 1 --Es activo en 1
FROM @xmlData.nodes('root/Articulos/Articulo') as T(Item)