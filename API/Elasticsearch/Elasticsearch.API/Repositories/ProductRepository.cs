﻿using Elasticsearch.API.Models;
using Nest;
using System.Collections.Immutable;

namespace Elasticsearch.API.Repositories
{
    public class ProductRepository
    {
        private readonly ElasticClient _client;
        private const string IndexName = "products";
        public ProductRepository(ElasticClient client)
        {
            _client = client;
        }

        public async Task<Product?> SaveAsync(Product newProduct)
        {
            newProduct.Created = DateTime.Now;

            var response = await _client.IndexAsync(newProduct, x => x.Index(IndexName));

            //FastFail
            if (!response.IsValid) return null;

            newProduct.Id = response.Id;
            return newProduct;
        }

        public async Task<ImmutableList<Product>> GetAllAsync()
        {
            var result = await _client.SearchAsync<Product>(
                s => s.Index(IndexName).Query(q => q.MatchAll()));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            var response = await _client.GetAsync<Product>(id, x => x.Index("products"));

            if (!response.IsValid)
                return null;

            response.Source.Id = response.Id;

            return response.Source;
        }
    }
}