using System.Text;
using ElasticDotnet.Application.Validators;
using ElasticDotnet.Domain.Models;
using Elasticsearch.Net;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace ElasticDotnet.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        services.AddScoped<IValidator<AuthDTO>, AuthValidator>();

        services.AddSingleton<IElasticClient>(_ =>
        {
            #region Elastic in the cloud
            // use this part of the code to connect to an elasticsearch cluster in the cloud,
            // provide cloudId and password to this AddApplication method

            // var settings = new ConnectionSettings(
            //         new CloudConnectionPool(elasticCloudId,
            //             new BasicAuthenticationCredentials("elastic", elasticPassword))
            //     )
            //     .DisableDirectStreaming()
            //     .DefaultMappingFor<Product>(m => m
            //         .PropertyName(p => p.ProductName, "ProductName")
            //         .PropertyName(p => p.Description, "Description")
            //     )
            // .OnRequestCompleted(response =>
            //     {
            //         Console.WriteLine($"Request: {Encoding.UTF8.GetString(response.RequestBodyInBytes)}");
            //         Console.WriteLine($"Response: {Encoding.UTF8.GetString(response.ResponseBodyInBytes)}");
            //     });
            #endregion

            #region Elasticsearch with docker-compose
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                .BasicAuthentication("elastic", "123456789")
                .DisableDirectStreaming()
                .DefaultMappingFor<AxamProduct>(m => m
                    .PropertyName(p => p.CodeInterne, "code interne")
                    .PropertyName(p => p.ImageProduit, "image produit")
                    .PropertyName(p => p.CodeABarre, "code a barre")
                    .PropertyName(p => p.REFERENCE, "REFERENCE")
                    .PropertyName(p => p.SKU, "SKU")
                    .PropertyName(p => p.LabelProduit, "label produit")
                    .PropertyName(p => p.SEOLabelProduit, "SEO label produit")
                    .PropertyName(p => p.Categorie, "categorie")
                    .PropertyName(p => p.SousCategorie, "sous-categorie")
                    .PropertyName(p => p.SousSousCategorie, "sous-sous-categorie")
                    .PropertyName(p => p.CategorieId, "categorie_id")
                    .PropertyName(p => p.Collection, "collection")
                    .PropertyName(p => p.BrèveDescription, "Brève description")
                    .PropertyName(p => p.Description, "Description")
                    .PropertyName(p => p.Tags, "Tags")
                    .PropertyName(p => p.FicheTechnique, "fiche technique")
                    .PropertyName(p => p.AltImage, "alt image(71 caracteres)")
                    .PropertyName(p => p.Link, "link")
                    .PropertyName(p => p.MetaDescription, "meta-description")
                    .PropertyName(p => p.MetaTitle, "meta title")
                    .PropertyName(p => p.OldOptimizationGrade, "old_optimization grade")
                    .PropertyName(p => p.NewOptimizationGrade, "new_optimization grade")
                    .PropertyName(p => p.Poids, "Poids")
                    .PropertyName(p => p.Couleur, "Couleur")
                    .PropertyName(p => p.ColorId, "color_id")
                    .PropertyName(p => p.Marque, "Marque")
                    .PropertyName(p => p.MarqueId, "marque_id")
                    .PropertyName(p => p.Garantie, "garantie")
                    .PropertyName(p => p.Stock, "Stock")
                    .PropertyName(p => p.FabriqueEn, "fabriqué en")
                    .PropertyName(p => p.EstRetournable, "est retournable")
                    .PropertyName(p => p.PrixVendeur, "Prix vendeur")
                    .PropertyName(p => p.PrixBrute, "Prix brute")
                    .PropertyName(p => p.PrixPromo, "Prix Promo")
                    .PropertyName(p => p.LienWebEtVideo, "lien (web et video )\n")
                    .PropertyName(p => p.ImagePrincipale, "image principale")
                    .PropertyName(p => p.ImagesSecondaires, "images secondaires")
                    .PropertyName(p => p.SellerId, "seller-id")
                    .PropertyName(p => p.CreatedBy, "Created by")
            )
            .OnRequestCompleted(response =>
                {
                    Console.WriteLine($"Request: {Encoding.UTF8.GetString(response.RequestBodyInBytes)}");
                    Console.WriteLine($"Response: {Encoding.UTF8.GetString(response.ResponseBodyInBytes)}");
                });

            return new ElasticClient(settings);
            #endregion
        });

        return services;
    }
}


