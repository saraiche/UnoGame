# Estándar de desarrollo
## Introducción
Este proyecto se desarrollará en C# Netframework. Para la comunicación en red se utilizará Windows Communication Foundation (WCF) y para la capa de presentación se utilizará Windows Presentation Foundation (WPF).
## Propósito
El presente documento sirve como guía para los códigos fuentes del videojuego a realizar para la asignatura de Tecnologías para la construcción de software de la Facultad de Estadística e Informática. Tiene como propósito que la comunicación del equipo por medio del código sea efectiva. Busca unificar el código a escribir, así como dar una guía de la forma en que serán representados los distintos componentes del código.
Nombrado
## Nombrado
### Reglas generales
Todo el código fuente será escrito en el idioma inglés, incluyendo el nombre de métodos, atributos, propiedades.
Se usan nombres descriptivos y tienen relación con la función que desempeñan (preferencia a nombres largos y descriptivos que cortos y poco entendibles).
#### Bien
```csharp
public bool ValidateEmail(string EmailToValidate);
```
#### Mal
```csharp
public bool validateE(string e);
```
### Variables
* Se usa UpperCammelCase
* Se usan frases nominales o palabras completas para describir explícitamente los datos que almacenarán
* Acrónimos son usados como palabras para el UpperCamelCase (ej: DireccionUrl no DireccionURL)
* Todas las variables son inicializadas en sus puntos de declaración
* Se hace una declaración de variable por línea
#### Bien
```csharp
string Name = "";
string Username = "";
```
#### Mal
```csharp
string name;
string u;
string name, u;
```
### Propiedades
Si las propiedades tienen modificadores de acceso públicos, serán escritos en notación UpperCammelCase.
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
* Todos los metodos se escriben en UpperCammelCase
* Se sigue el esquema de nombrado:
tipoDeDatoDeRetorno NombreDelMetodo(Parametros);
### Bien
```csharp
public async SendMessage(string User, string Message)
{
    await Clients.All.SendAsync("ReceiveMessage", User, Message);
}
```
### Mal
```csharp
public async sendMessage(string user, string message)
{
    await Clients.All.SendAsync("ReceiveMessage", user, message);
}
```
### Nombrado de acciones
**acción clic**
Es el nombre del controlador seguido de _Click
```csharp
private void BtnSignUp_Click(object sender, RoutedEventArgs e)
{
}
```
### Nombrado de nombre de etiquetas (Prefijos)
**labels**
Para el nombrado de los componentes de interfaz gráfica se utiliza como prefijo la(s) inicial(es) del componente en mayúscula seguido del nombre de la variable con la inicial mayúscula (estilo upper camel case)
#### Bien
```xaml
<Label x:Name="LblUno"/>
```
#### Mal
```xaml
<Label x:Name="TlUno"/>
```
**buttons**
#### Bien
```xaml
<button x:Name="BtnUno/>
```
#### Mal
```xaml
<button x:Name="botonUno"/>
```
**TextBox**
#### Bien
```xaml
<TextBox x:Name="TbUno"/>
```
#### Mal
```xaml
<TextBox x:Name="textBoxUno"/>
```
**passwordBox**
#### Bien
```xaml
<PasswordBox  x:Name="PbUno"/>
```
#### Mal
```xaml
<PasswordBox  x:Name="passwordtextBoxUno"/>
```
### Constantes
*	Se usa la notación UPPER_SNAKE_CASE (todas las leras mayúsculas, cada palabra se separa de la siguiente por medio de un guión bajo)
### Bien
```csharp
const string PLAYER_USERNAME = "Saraiche";
```
### Mal
```csharp
const string playerUsername = "Saraiche";
```
### Clases
* Se usa la notación UpperCammelCase
### Bien
```csharp
public class Security 
{
    
}
```
### Mal
```csharp
public class security {
}
```

### Interfaces
Las interfaces tienen el prefijo "I"
### Bien
```csharp
public interface IUserManagment 
{
}
```
### Mal
```csharp
public interface userManagment {
}
```

## Estilo
### Identación
* Todo el código desarrollado tendrá identación de un tab.
### Bien
```csharp
public class Main{
	public static void main(String[] args){
		Console.Writeline(“Hola Mundo”);
	}
}
```
### Mal
```csharp
public class Main{
  public static void main(String[] args){
  Console.WriteLine(“Hola Mundo”);
  }
}
```
### Llaves
Cuando abrimos llaves en un metodo las llaves irán seguidos de un salto de linea en el metodo
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
Tiene que existir un espacio entre cada igual o entre variables o números y símbolos aritméticos o lógicos.
#### Bien
```
flag = true;
double counter = 0;
counter = counter + 1;
```
#### Mal
```csharp
flag=true;
counter=counter+1;
```
## Comentarios
### Formato
Se utilizan '///' para comentarios de una línea y '/** ' para dos líneas o más.
#### Bien
```
/// this line explains something

/**
these lines explains
something more
**/
```
#### Mal
```csharp
//this not explain something
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
