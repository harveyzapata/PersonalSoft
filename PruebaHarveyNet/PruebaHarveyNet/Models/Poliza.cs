using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace PruebaHarveyNet.Models;

public class Poliza
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string NumberPoliza { get; set; } = null!;
    public string NameClient { get; set; } = null!;
    public double Identification { get; set; }
    public DateTime Birthdate { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Fecha de Inicio de Vigencia")]
    [Required(ErrorMessage = "La fecha de inicio de vigencia es obligatoria.")]
    [DateNotInPast(ErrorMessage = "La fecha de inicio de vigencia debe estar vigente.")]
    public DateTime DateCreatePoliza { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Fecha de Fin de Vigencia")]
    [Required(ErrorMessage = "La fecha de fin de vigencia es obligatoria.")]
    [DateNotInPast(ErrorMessage = "La fecha de fin de vigencia debe estar vigente.")]
    //[DateGreaterThan("FechaInicioVigencia", ErrorMessage = "La fecha de fin de vigencia debe ser posterior a la fecha de inicio.")]
    public DateTime DateEndPoliza { get; set; }
    public string Coverage { get; set; } = null!;

    [Range(0, double.MaxValue, ErrorMessage = "El monto debe ser un valor positivo.")]
    public double MaximumValuePoliza { get; set; }
    public string PolizaPlanName { get; set; } = null!;
    public string CityClient { get; set; } = null!;
    public string AddressClient { get; set; } = null!;
    public string LicensePlate { get; set; } = null!;
    public string ModelVehicle { get; set; } = null!;
    public Boolean InspectionVehicle { get; set; }

    public Boolean ValidateDates(DateTime dateIni, DateTime dateFin)
    {
        //    DateTime DateCreated = DateTime.Parse(dateIni);
        //    DateTime DateEnd = DateTime.Parse(dateFin);

        return dateFin > dateIni;
    }  
}



