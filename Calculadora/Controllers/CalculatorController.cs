using Microsoft.AspNetCore.Mvc;
using Calculadora.Models;

namespace Calculadora.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalculatorController : ControllerBase
{
    // GET api/calculator/add?a=1&b=2
    [HttpGet("add")]
    public ActionResult<CalcResponse> Add([FromQuery] double a, [FromQuery] double b)
        => Ok(new CalcResponse("add", a, b, a + b));

    // GET api/calculator/subtract?a=5&b=3
    [HttpGet("subtract")]
    public ActionResult<CalcResponse> Subtract([FromQuery] double a, [FromQuery] double b)
        => Ok(new CalcResponse("subtract", a, b, a - b));

    // GET api/calculator/multiply?a=2&b=4
    [HttpGet("multiply")]
    public ActionResult<CalcResponse> Multiply([FromQuery] double a, [FromQuery] double b)
        => Ok(new CalcResponse("multiply", a, b, a * b));

    // GET api/calculator/divide?a=8&b=2
    [HttpGet("divide")]
    public ActionResult<CalcResponse> Divide([FromQuery] double a, [FromQuery] double b)
    {
        if (b == 0)
            return Problem("No se puede dividir entre cero.", statusCode: StatusCodes.Status400BadRequest);

        return Ok(new CalcResponse("divide", a, b, a / b));
    }

    // POST api/calculator
    // Body: { "op": "add|subtract|multiply|divide", "a": 1, "b": 2 }
    [HttpPost]
    public ActionResult<CalcResponse> Operate([FromBody] CalcRequest req)
    {
        var op = req.Op?.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(op))
            return Problem("El campo 'op' es obligatorio.", statusCode: StatusCodes.Status400BadRequest);

        return op switch
        {
            "add" or "+"       => Ok(new CalcResponse("add", req.A, req.B, req.A + req.B)),
            "subtract" or "-"  => Ok(new CalcResponse("subtract", req.A, req.B, req.A - req.B)),
            "multiply" or "*"  => Ok(new CalcResponse("multiply", req.A, req.B, req.A * req.B)),
            "divide" or "/"    => req.B == 0
                                    ? Problem("No se puede dividir entre cero.", statusCode: StatusCodes.Status400BadRequest)
                                    : Ok(new CalcResponse("divide", req.A, req.B, req.A / req.B)),
            _ => Problem("Operación no soportada. Usa: add, subtract, multiply, divide.", statusCode: StatusCodes.Status400BadRequest)
        };
    }
}
