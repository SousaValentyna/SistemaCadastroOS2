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
    public async Task<IActionResult> Index(string searchString)
    {
        // Mantém o valor da pesquisa atual na ViewData para que o campo de pesquisa mantenha o valor
        ViewData["CurrentFilter"] = searchString;

        // Recupera todos os OSModels do banco de dados
        var osModels = from os in _context.OSModels
                       select os;

        // Se a string de busca não for nula ou vazia, filtra os resultados
        if (!string.IsNullOrEmpty(searchString))
        {
            osModels = osModels.Where(s => s.NumeroOS.ToString().Contains(searchString)
                                           || s.TituloServico.Contains(searchString));
        }

        // Retorna a View com os dados filtrados ou todos os dados se a string de busca estiver vazia
        return View(await osModels.ToListAsync());
    }

    // GET: OS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: OS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TituloServico,CNPJ,NomeDoCliente,CPF,NomeDoPrestador,DataExecucaoServico,ValorDoServico")] OSModel oSModel)
    {
        if (ModelState.IsValid)
        {
            // Ajuste de timezone
            oSModel.DataExecucaoServico = DateTime.SpecifyKind(oSModel.DataExecucaoServico, DateTimeKind.Utc);

            // Gerar o NumeroOS automaticamente
            oSModel.NumeroOS = await GenerateNumeroOS();

            _context.Add(oSModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(oSModel);
    }

    // GET: OS/Edit/5
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

    // POST: OS/Edit/5
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
                // Ajuste de timezone
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
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(oSModel);
    }

    // GET: OS/Delete/5
    public async Task<IActionResult> Deletar(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var oSModel = await _context.OSModels
            .FirstOrDefaultAsync(m => m.NumeroOS == id);
        if (oSModel == null)
        {
            return NotFound();
        }

        return View(oSModel);
    }

    // POST: OS/Delete/5
    [HttpPost, ActionName("Deletar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var oSModel = await _context.OSModels.FindAsync(id);
        _context.OSModels.Remove(oSModel);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // Método auxiliar para verificar se o modelo existe
    private bool OSModelExists(int id)
    {
        return _context.OSModels.Any(e => e.NumeroOS == id);
    }

    // Método para gerar o NumeroOS
    private async Task<int> GenerateNumeroOS()
    {
        // Aqui você pode gerar um novo número sequencial baseado no maior número atual
        var maxNumeroOS = await _context.OSModels.MaxAsync(o => (int?)o.NumeroOS) ?? 0;
        return maxNumeroOS + 1;
    }
}