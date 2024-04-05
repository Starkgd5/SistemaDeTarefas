using Microsoft.EntityFrameworkCore;
using SistemaDeTarefas.Data;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Repositorios.Interfaces;

namespace SistemaDeTarefas.Repositorios
{
    public class TarefaRepositorio : ITarefaRepositorio
    {
        private readonly SistemaTarefasDBContext _dBContext;
        public TarefaRepositorio(SistemaTarefasDBContext sistemaTarefasDBContext)
        {
            _dBContext = sistemaTarefasDBContext;
        }

        public async Task<TarefaModel> BuscarPorId(int id)
        {
            return await _dBContext.Tarefas.Include(x => x.Usuario)
                .FirstOrDefaultAsync(tarefa => tarefa.Id == id);
        }

        public async Task<List<TarefaModel>> BuscarTodasTarefas()
        {
            return await _dBContext.Tarefas.Include(x => x.Usuario).ToListAsync();
        }
        public async Task<TarefaModel> Adicionar(TarefaModel tarefa)
        {
            await _dBContext.Tarefas.AddAsync(tarefa);
            await _dBContext.SaveChangesAsync();

            return tarefa;
        }
        public async Task<TarefaModel> Atualizar(TarefaModel tarefa, int id)
        {
            TarefaModel tarefaPorId = await BuscarPorId(id);
            if (tarefaPorId == null)
            {
                throw new Exception($"Tarefa: id {id} não existe");
            }

            tarefaPorId.Nome = tarefa.Nome;
            tarefaPorId.Description = tarefa.Description;
            tarefaPorId.Status = tarefa.Status;
            tarefaPorId.UsuarioId = tarefa.UsuarioId;
            _dBContext.Tarefas.Update(tarefaPorId);
            await _dBContext.SaveChangesAsync();
            return tarefaPorId;
        }

        public async Task<bool> Apagar(int id)
        {
            TarefaModel tarefaPorId = await BuscarPorId(id);
            if (tarefaPorId == null)
            {
                throw new Exception($"Tarefa: id {id} não existe");
            }
            _dBContext.Tarefas.Remove(tarefaPorId);
            await _dBContext.SaveChangesAsync();
            return true;
        }
    }
}
