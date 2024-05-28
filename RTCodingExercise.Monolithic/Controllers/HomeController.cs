using RTCodingExercise.Monolithic.Domain;
using RTCodingExercise.Monolithic.Models;
using System.Diagnostics;

namespace RTCodingExercise.Monolithic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var plates = await _context.Plates.ToListAsync();

            _context.PlatesWithSalesMarkup(plates);

            return View(plates.OrderBy(x => x.SalePrice));
        }

        // GET: Transaction/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View("~/Views/Plate/Create.cshtml");
        }

        // POST: Transaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddPlateViewModel addPlateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Index));
            }

            var plate = new Plate()
            {
                Id = Guid.NewGuid(),
                Registration = addPlateViewModel.Registration,
                PurchasePrice = addPlateViewModel.PurchasePrice,
                SalePrice = addPlateViewModel.SalePrice,
                Letters = addPlateViewModel.Letters,
                Numbers = addPlateViewModel.Numbers
            };

            _context.Plates.OrderBy(x => x.SalePrice == plate.SalePrice);
            _context.Plates.Add(plate);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}