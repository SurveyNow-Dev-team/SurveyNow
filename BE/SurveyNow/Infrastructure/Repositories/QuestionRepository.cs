﻿using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class QuestionRepository: BaseRepository<Question>, IQuestionRepository
{
    public QuestionRepository(AppDbContext context, ILogger<BaseRepository<Question>> logger) : base(context, logger)
    {
    }
}