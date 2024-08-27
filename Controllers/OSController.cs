using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class OSController : Controller
{   
    private readonly ApplicationDbContext _context;
    
    public OSController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Método para filtrar e exibir a tela Index
    public async Task<IActionResult> Index(string searchString)
    {
        ViewData["CurrentFilter"] = searchString;

        var osModels = from os in _context.OSModels
                       select os;

        if (!string.IsNullOrEmpty(searchString))
        {
            osModels = osModels.Where(s => s.NumeroOS.ToString().Contains(searchString)
                                           || s.TituloServico.Contains(searchString));
        }

        return View(await osModels.ToListAsync());
    }

    // Método para renderizar a View Create
    public IActionResult Create()
    {
        return View();
    }

    // Método para criar uma nova OS
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TituloServico,CNPJ,NomeDoCliente,CPF,NomeDoPrestador,DataExecucaoServico,ValorDoServico")] OSModel oSModel)
    {
        if (ModelState.IsValid)
        {
            oSModel.DataExecucaoServico = DateTime.SpecifyKind(oSModel.DataExecucaoServico, DateTimeKind.Utc);
            oSModel.NumeroOS = await GenerateNumeroOS();
            _context.Add(oSModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(oSModel);
    }

    // Método para renderizar a tela de Edição
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var oSModel = await _context.OSModels.FindAsync(id);
        if (oSModel == null)
        {
            return NotFound();
        }
        return View(oSModel);
    }

    // Método para editar
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("NumeroOS,TituloServico,CNPJ,NomeDoCliente,CPF,NomeDoPrestador,DataExecucaoServico,ValorDoServico")] OSModel oSModel)
    {
        if (id != oSModel.NumeroOS)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                oSModel.DataExecucaoServico = DateTime.SpecifyKind(oSModel.DataExecucaoServico, DateTimeKind.Utc);
                _context.Update(oSModel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OSModelExists(oSModel.NumeroOS))
                {
                    return NotFound();
                }
                else
                {
                    throw; // exibe a mensagem de erro
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(oSModel);
    }

    // Método para renderizar a tela de deletar
    public async Task<IActionResult> Deletar(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var oSModel = await _context.OSModels.FirstOrDefaultAsync(m => m.NumeroOS == id);
        if (oSModel == null)
        {
            return NotFound();
        }

        return View(oSModel);
    }

    // Método para deletar um registro
    [HttpPost, ActionName("Deletar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var oSModel = await _context.OSModels.FindAsync(id);
        _context.OSModels.Remove(oSModel);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool OSModelExists(int id)
    {
        return _context.OSModels.Any(e => e.NumeroOS == id);
    }

    private async Task<int> GenerateNumeroOS()
    {
        var maxNumeroOS = await _context.OSModels.MaxAsync(o => (int?)o.NumeroOS) ?? 0;
        return maxNumeroOS + 1;
    }
}