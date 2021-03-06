using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestinaryWebApp.Dto;
using QuestinaryWebApp.Extensions;
using QuestionnaireWebApp.Core;
using QuestionnaireWebApp.Core.Models;
using QuestionnaireWebApp.Models;

namespace QuestionnaireWebApp.Controllers;

public class ConstructorController : Controller //Унаследовались от Контроллера, что бы наш класс являлся Контролером
{
    private readonly QuestionnaireContext _context; //Переменная символизирующая БД
    private readonly ILogger<ConstructorController> _logger;
    private readonly IMapper _mapper; //переменная Автомаппера


    public ConstructorController(
        QuestionnaireContext context,
        IMapper mapper,
        ILogger<ConstructorController> logger
    ) //Конструктор класса Контролера в котором внедряем...
    {
        _context = context; // зависимости БД
        _mapper = mapper; // зависимости Маппера
        _logger = logger;
    }

    [HttpGet] //атрибут метода контролера
    public IActionResult Index() //Медод контроллера для открытия страницы ОПРОСНИКа
    {
        return View(); //Возвращает страницу
    }

    [HttpGet("[controller]/[action]/{questionnaireId}")] //атрибут метода контролера с конкретной инструкцией вызова
    public IActionResult
        Index(int questionnaireId) // Медод контроллера для открытия страницы ОПРОСНИКа для РЕДАКТИРОВАНИЯ существующих опросников
    {
        try
        {
            var questionnaire = _context.Questionnairees //создали переменную Опросника и выдергиваем в нее из бд...
                .Include(x => x.Questions)
                .ThenInclude(x => x.AnswerVariants)
                .First(x => x.Id == questionnaireId); //...опросник с ИД таким же как во входном параметре
            var questionnaireDto = _mapper.Map<QuestionnaireDto>(questionnaire); //маппим полученый опросник до ДТОшки
            return View(
                new
                    ConstructorViewModel() //Возвращаем СТРАНИЦУ(view) на основе Модели страницы, которая основана на нашей ДТОшки опросника
                    {
                        Questionnaire = questionnaireDto
                    });
        }
        catch (Exception e)
        {
            _logger.Error(e);
            return StatusCode((int) HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost] //атрибут метода контролера
    public void Save(QuestionnaireDto input) //Метод сохранения опросника (дискета)
    {
        try
        {
            var questionnaire =
                _mapper.Map<Questionnaire>(input); //Замапили, получили опросник из входного параметра в переменную

            var entity = _context
                .Questionnairees //нашли такой же опросник в БД и положили данные из БД в новую переменную
                .Include(x => x.Questions)
                .AsNoTracking() //Не отслеживать сущности
                .FirstOrDefault(x => x.Id == questionnaire.Id);

            if (entity is not null) //если такой опросник нашелся и в переменной он есть, то...
            {
                entity.Name =
                    questionnaire.Name; //в имя переменной с данными из БД присваиваем имя опросника из вх.параметра

                foreach (var question in entity.Questions) //в опроснике БД добираемся до каждого вопроса
                    _context.Questions.Remove(question); //в таблице БД-ВопросЫ удаляем каждый из найденых вопросов

                AddQuestions(input,
                    entity.Id); //метод сохранения вопросов в таблице вопросов БД - наш старый(вынесеный) метод, который принимает новые данные из Инпута + ИД
                _context.SaveChanges(); //Сохраняем изменения в БД
            }
            else //иначе, если если переданного опросника небыло в БД то...
            {
                _context.Questionnairees
                    .Add(questionnaire); //то просто кладем в БД новый опросник(из нашей смапиной переменной)-(точнее пока что даем только имя новому опроснику)
                _context.SaveChanges(); //Сохраняем изменения в БД
            }
        }
        catch (Exception e)
        {
            _logger.Error(e);
        }
    }

    private void
        AddQuestions(QuestionnaireDto input,
            int questionnaireId) //вспомогательный метод сохранения вопросов в таблице вопросов БД
    {
        try
        {
            var num = 1; //переменная счетчика для номеров вопроса

            foreach (var questionDto in input.Questions) //перебераем Вопросы пришедшие во входном параметре
            {
                var question = new Question() //кладем-маппим данные каждого вопроса во временную переменную
                {
                    Number = num++,
                    Type = questionDto.Type,
                    Text = questionDto.Name,
                    QuestionnaireeId = questionnaireId
                };

                var answerNum = 1;

                _context.Questions.Add(question); //в цикле кладем каждый вопрос в таблицу БД - Вопросы
                _context.SaveChanges(); //сохраняем изменения

                if (questionDto.VariantAnswers is null) //если в Вопросе нет вариантов ответа то покидаем итерацию
                    continue;

                foreach (var variantAnswer in questionDto.VariantAnswers) //если в Вопросе есть варианты ответа то...
                {
                    _context.Answers.Add(new Answer() //в цикле добавляем каждый вариант ответа в таблицу Ответов
                    {
                        Text = variantAnswer.Name,
                        Number = answerNum++,
                        QuestionId = question.Id
                    });

                    _context.SaveChanges(); //сохраняем изменения
                }
            }

            _context.SaveChanges(); //сохраняем изменения опираций сделаных в методе
        }
        catch (Exception e)
        {
            _logger.Error(e);
            throw e;
        }
    }
}