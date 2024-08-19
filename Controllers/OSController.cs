using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


public class OSController : Controller
{
    private readonly ApplicationDbContext _context;

    public OSController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: OS
    public async Task<IActionResult> Index()
    {
        return View(await _context.OSModels.ToListAsync());
    }

    // GET: OS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: OS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NumeroOS,TituloServico,CNPJ,NomeDoCliente,CPF,NomeDoPrestador,DataExecucaoServico,ValorDoServico")] OSModel oSModel)
    {
        if (ModelState.IsValid)
        {
            // Descobrir uma forma melhor de resolver problema de timezone
            oSModel.DataExecucaoServico = DateTime.SpecifyKind(oSModel.DataExecucaoServico, DateTimeKind.Utc);

            _context.Add(oSModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(oSModel);
    }
}