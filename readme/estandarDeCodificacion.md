# Estandar de desarrollo
## Introduccion
Este proyecto se desarrollará en C#  netframework para la comunicación en red se utilizará Windows communication foundatio y para la capa de presentación se utilizará Windows Presentation Foundation
## Proposito
El presente documento sirve como guía para los códigos fuentes del videojuego a realizar para la asignatura de Tecnologías para la construcción de software de la Facultad de Estadística e Informática. Tiene como propósito que la comunicación del equipo por medio del código sea efectiva. Busca unificar el código a escribir, así como dar una guía de la forma en que serán representados los distintos componentes del código.
Nombrado
## Nombrado
### Reglas generales
Todo el código fuente será escrito en el idioma inglés, incluyendo el nombre de métodos, atributos, propiedades.
Si las propiedades tienen modificadores de acceso públicos, serán escritos en notación UpperCammelCase.

### Propiedades
#### Bien
```csharp
public string FirstName{ get; set; }
```
#### Mal
```csharp
public string getFisrtName()
{
    return this.firstName;
}
```
### Metodos
Todos los metodos se escriben en UpperCammelCase
```csharp
public async SendMessage(string user, string message)
{
    await Clients.All.SendAsync("ReceiveMessage", user, message);
}
```
### Nombrado de acciones
**acción clic**
Es el nombre del controlador seguido de _Click
```csharp
private void btnSignUp_Click(object sender, RoutedEventArgs e)
{
}
```
## Estilo
### Llaves
Cuando abrimos llaves en un metodo las llaves iran seguidos de un salto de linea en el metodo
#### Bien
```csharp
public CustomerDto ToDto()
{
    return new CustomerDto()
    {
        Address = Address,
        Email = Email,
        FirstName = FirstName,
        LastName = LastName,
        Phone = Phone,
        Id = Id ?? throw new Exception("el id no puede ser null")
    };
}
```
#### Mal
```csharp
public CustomerDto ToDto() {
    return new CustomerDto() {
        Address = Address,
        Email = Email,
        FirstName = FirstName,
        LastName = LastName,
        Phone = Phone,
        Id = Id ?? throw new Exception("el id no puede ser null")
    };
}
```
### Espaciados
Tiene que existir un espacio entre cada igual
#### Bien
```
flag = true;
```
#### Mal
```csharp
flag=true;
```
### Nombrado de nombre de etiquetas (Prefijos)
**labels**
#### Bien
```xaml
<Label x:Name="lblUno"/>
```
#### Mal
```xaml
<Label x:Name="tlUno"/>
```
**buttons**
#### Bien
```xaml
<button x:Name="btnUno/>
```
#### Mal
```xaml
<button x:Name="botonUno"/>
```
**TextBox**
#### Bien
```xaml
<TextBox x:Name="tbUno"/>
```
#### Mal
```xaml
<TextBox x:Name="textBoxUno"/>
```
**passwordBox**
#### Bien
```xaml
<PasswordBox  x:Name="pbUno"/>
```
#### Mal
```xaml
<PasswordBox  x:Name="passwordtextBoxUno"/>
```
## Internacionalizacion
El xmnls debera de hacer referencia a la palabra p de propiertes
### Xmnls
#### bien
```xml
xmlns:p="clr-namespace:unoProyect.Properties"
```
#### Mal
```xml
xmlns:internacionalizacion="clr-namespace:unoProyect.Properties"
```
### Labels content
#### bien
```xaml
<Label x:Name = "lblName" Content = "{x:Static p:Resources.Name}"/>
```
#### mal
```xaml
<Label x:Name = "lblName" Content = "Nombre"/>
```
### Metodos de localizacion
Los metodos deben de ser tipo publico y no internal
#### bien
```csharp
        public static string chat {
            get {
                return ResourceManager.GetString("chat", resourceCulture);
            }
        }
```
#### mal
```csharp
        internal static string chat {
            get {
                return ResourceManager.GetString("chat", resourceCulture);
            }
        }
```