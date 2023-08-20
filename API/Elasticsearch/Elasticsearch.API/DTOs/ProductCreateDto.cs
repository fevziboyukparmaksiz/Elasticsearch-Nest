﻿using Elasticsearch.API.Models;

namespace Elasticsearch.API.DTOs
{
    public record ProductCreateDto(string Name, decimal Price, int stock, ProductFeatureDto Feature)
    {

        public Product CreateProduct()
        {
            return new Product
            {
                Name = Name,
                Price = Price,
                Stock = stock,
                Feature = new ProductFeature()
                {
                    Color = (EColor)int.Parse(Feature.Color.ToString()),
                    Width = Feature.Width,
                    Height = Feature.Height
                }
            };
        }
    }
}
