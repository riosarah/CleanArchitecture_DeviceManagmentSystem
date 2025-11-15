using Api.Extensions;
using Application.Common.Results;
using Application.Features.Device.Commands.CreateDevice;
using Application.Features.Device.Commands.UpdateDevice;
using Application.Features.Device.Queries.GetAllDevices;
using Application.Features.Device.Queries.GetDeviceById;
using Application.Features.Dtos;
using Application.Features.Sensors.Commands.CreateSensor;
using Application.Features.Sensors.Commands.DeleteSensor;
using Application.Features.Sensors.Commands.UpdateSensor;
using Application.Features.Sensors.Queries.GetAllSensors;
using Application.Features.Sensors.Queries.GetSensorById;
using Application.Features.Sensors.Queries.GetSensorsWithCounts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController(IMediator mediator) : ControllerBase
    {

        /// <summary>
        /// Liefert alle Sensoren sortiert nach Location und Name.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetDeviceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            // Über MediatR die Query ausführen und DTOs aus der Anwendungsschicht abrufen
            var result = await mediator.Send(new GetAllDevicesQuery(), ct);
            return result.ToActionResult(this);
        }

        /// <summary>
        /// Liefert einen Sensor per Id, falls vorhanden.
        /// Sonst 404 Not Found.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(GetSensorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetDeviceByIdQuery(id), ct);
            return result.ToActionResult(this);
        }

        

        /// <summary>
        /// Legt einen neuen Sensor an.
        /// </summary>
        /// <remarks>
        /// Regeln:
        /// - Name mindestens 2 Zeichen
        /// - Name darf nicht der Location entsprechen (case-insensitive)
        /// - Kombination (Location, Name) muss eindeutig sein
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(GetDeviceDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateDeviceCommand command, CancellationToken ct)
        {
            // Erstellung anstoßen; Result in passenden HTTP-Status umwandeln (Created etc.)
            var result = await mediator.Send(command, ct);
            return result.ToActionResult(this, createdAtAction: nameof(GetById),
                routeValues: new { id = result?.Value?.Id });
        }

        /// <summary>
        /// Aktualisiert einen Sensor.
        /// </summary>
        /// <remarks>
        /// Regeln wie beim Erstellen: Name min. 2, Name != Location, (Location, Name) eindeutig.
        /// </remarks>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(GetDeviceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDeviceCommand command, CancellationToken ct)
        {
            if (id != command.Id)
            {
                // Konsistente Rückgabe ohne Exception
                Result<GetDeviceDto> badResult = Result<GetDeviceDto>.ValidationError(
                    "The route ID does not match the sensor ID in the request body.");
                return badResult.ToActionResult(this);
            }
            // Update ausführen. Das Result wird in einen HTTP-Response gemappt.
            var result = await mediator.Send(new UpdateDeviceCommand(id, command.name, command.serialNumber, command.type), ct);
            return result.ToActionResult(this);
        }

        /// <summary>
        /// Löscht einen Sensor.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new DeleteSensorCommand(id), ct);
            return result.ToActionResult(this);
        }
    }
}
