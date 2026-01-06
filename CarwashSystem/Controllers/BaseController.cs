using Microsoft.AspNetCore.Mvc;
using UI.Models;

public class BaseController : Controller
{
    protected readonly CarwashSystemContext _context;

    public BaseController(CarwashSystemContext context)
    {
        _context = context;
    }

    protected bool CajaAbierta()
    {
        return _context.Caja.Any(c => c.Estado == "ABIERTA");
    }
}