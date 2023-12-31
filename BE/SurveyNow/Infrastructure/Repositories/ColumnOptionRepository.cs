﻿using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class ColumnOptionRepository:BaseRepository<ColumnOption>, IColumnOptionRepository
{
    public ColumnOptionRepository(AppDbContext context, ILogger<BaseRepository<ColumnOption>> logger) : base(context, logger)
    {
    }
}