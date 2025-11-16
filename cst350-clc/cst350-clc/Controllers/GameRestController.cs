using cst350_clc.Models.GameSave;
using cst350_clc.Services.GameSaveService;
using Microsoft.AspNetCore.Mvc;

namespace cst350_clc.Controllers
{
	[ApiController]
	[Route("api/v1/gameSaves")]
	public class GameRestController : ControllerBase
	{
		private readonly ILogger<GameRestController> _logger;
		private readonly IGameSaveService _gameSaveService;

		public GameRestController(ILogger<GameRestController> logger, IGameSaveService gameSaveService)
		{
			_logger = logger;
			_gameSaveService = gameSaveService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllProducts()
		{
			IEnumerable<GameSaveModel> saves = await _gameSaveService.GetAllGameSaves();
			return Ok(saves); // 200
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<GameSaveModel>> GetSaveById(int id)
		{
			var save = await _gameSaveService.GetGameSaveById(id);
			if (save == null)
				return NotFound(); // 404 - no save found by that ID
			return Ok(save); // 200 - returns found save
		}

		[HttpGet("user/{userId}")]
		public async Task<IActionResult> GetSavesByUserId(int userId)
		{
			var save = await _gameSaveService.GetGameSavesByUserId(userId);
			if (save == null)
				return NotFound(); // 404 - save not found
			return Ok(save); // 200 - save found with result
		}

		[HttpPost("create")]
		public async Task<IActionResult> CreateGameSave([FromForm] GameSaveModel gameSave)
		{
			if (ModelState.IsValid)
			{
				await _gameSaveService.AddGameSave(gameSave);
				return CreatedAtAction(nameof(GetSaveById), new {id = gameSave.Id}, gameSave); // 201
			} else
			{
				return BadRequest(); // 400
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateGameSave([FromForm] GameSaveModel gameSave)
		{
			if (ModelState.IsValid)
			{
				await _gameSaveService.UpdateGameSave(gameSave);
				return NoContent(); // 204 - successful no content
			} else
			{
				return BadRequest(); // 400 for failure
			}
		}
	}
}
