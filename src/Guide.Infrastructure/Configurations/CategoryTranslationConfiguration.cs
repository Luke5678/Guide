﻿using Guide.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Guide.Infrastructure.Configurations;

public class CategoryTranslationConfiguration : IEntityTypeConfiguration<CategoryTranslation>
{
    public void Configure(EntityTypeBuilder<CategoryTranslation> builder)
    {
        builder.Property(t => t.LanguageCode).HasMaxLength(2).IsRequired();
        builder.Property(t => t.Name).HasMaxLength(255).IsRequired();
    }
}