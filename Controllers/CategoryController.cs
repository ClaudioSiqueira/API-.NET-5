using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

[Route("/categories")]
public class CategoryController : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
    {
        try
        {          // AsNoTracking serve para pegar apenas informações necessarias
            var categories = await context.Categories.AsNoTracking().ToListAsync();
            return Ok(categories);
        }
        catch
        {
            return BadRequest(new { message = "Não foi possivel buscar as categorias" });
        }

    }

    [HttpGet]
    [Route("{id:int}")] // Passa um Id na rota e só pode ser inteiro, se for outra coisa da 404
    public async Task<ActionResult<Category>> GetById(
        int id,
        [FromServices] DataContext context)
    {
        try
        {
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return Ok(category);
        }
        catch
        {
            return BadRequest(new { message = "Não foi possivel buscar a categoria" });
        }

    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<Category>> POST(
        [FromBody] Category model,// Pega uma categoria do body, e verifica se é igual ao model Category
        [FromServices] DataContext context)  // Context é o meu banco de dados
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);  // Verifica se o usuario mandou um model da forma correta, se estiver errado, ele manda os erros que eu defini no ErrorMessage

        try
        {
            context.Categories.Add(model);  // Adicionando o model na tabela Categories do banco context(Database)
            await context.SaveChangesAsync();  // Salva as mudanças no banco
            return Ok(model);
        }
        catch
        {
            return BadRequest(new { message = "Não foi possivel criar categoria" });
        }

    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> DELETE(
        int id,
        [FromServices] DataContext context)
    {
        try
        {
            // Para deletar, primeiro é preciso buscar a categoria, nao da pra deletar direto
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id); // funcao que busca
            if (category == null)
                return NotFound(new { message = "Categoria não encontrada" });
            context.Categories.Remove(category);  // Deleta passando a categoria
            await context.SaveChangesAsync();
            return Ok(new { message = "Categoria removida com sucesso !" });
        }
        catch
        {
            return NotFound(new { message = "Não foi possivel deletar categoria" });
        }

    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> PUT(
        int id, [FromBody] Category model,
        [FromServices] DataContext context)
    {
        if (model.Id != id)
            return NotFound(new { message = "Categoria não encontrada" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            context.Entry<Category>(model).State = EntityState.Modified;  // O Entry que faz a função de editar no banco de dados, faz tudo altomatico, nao precisa informar a tabela, e nao precisa validar quais dados serao alterados, ele descobre sozinho
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch
        {
            return NotFound(new { message = "Não foi possivel editar categoria" });
        }

    }
}