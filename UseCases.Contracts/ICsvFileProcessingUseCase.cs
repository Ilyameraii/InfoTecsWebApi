namespace UseCases.Contracts;


/// <summary>
/// Use case обработки загруженного CSV-файла: парсинг, валидация,
/// сохранение строк в таблицу Values и агрегированных результатов в таблицу Results.
/// Если файл с таким именем уже существует, его данные перезаписываются.
/// </summary>
public interface ICsvFileProcessingUseCase
{
    /// <summary>
    /// Обрабатывает CSV-файл: парсит поток, валидирует строки,
    /// вычисляет агрегированные показатели и сохраняет результат в рамках одной транзакции.
    /// </summary>
    /// <param name="stream">Поток с содержимым CSV-файла.</param>
    /// <param name="fileName">Имя файла, используемое как ключ записей в БД.</param>
    Task ExecuteAsync(Stream stream, string fileName);
}