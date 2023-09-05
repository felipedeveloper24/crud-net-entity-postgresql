namespace CrudAPI.Models
{
    public class Persona
    {
        //Definimos los modelos con sus respectivos atributos
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Edad {  get; set; }
        public string? Correo { get; set; }
    }
}
