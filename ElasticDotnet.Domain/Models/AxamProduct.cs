namespace ElasticDotnet.Domain.Models;

using System.Text.Json.Serialization;

public sealed class AxamProduct
{
    [JsonPropertyName("code interne")]
    public string CodeInterne { get; set; }

    [JsonPropertyName("image produit")]
    public string ImageProduit { get; set; }

    [JsonPropertyName("code a barre")]
    public string CodeABarre { get; set; }

    [JsonPropertyName("REFERENCE")]
    public string REFERENCE { get; set; }

    [JsonPropertyName("SKU")]
    public string SKU { get; set; }

    [JsonPropertyName("label produit")]
    public string LabelProduit { get; set; }

    [JsonPropertyName("SEO label produit")]
    public string SEOLabelProduit { get; set; }

    [JsonPropertyName("categorie")]
    public string Categorie { get; set; }

    [JsonPropertyName("sous-categorie")]
    public string SousCategorie { get; set; }

    [JsonPropertyName("sous-sous-categorie")]
    public string SousSousCategorie { get; set; }

    [JsonPropertyName("categorie_id")]
    public int CategorieId { get; set; }

    [JsonPropertyName("collection")]
    public string Collection { get; set; }

    [JsonPropertyName("Brève description")]
    public string BrèveDescription { get; set; }

    [JsonPropertyName("Description")]
    public string Description { get; set; }

    [JsonPropertyName("Tags")]
    public string Tags { get; set; }

    [JsonPropertyName("fiche technique")]
    public string FicheTechnique { get; set; }

    [JsonPropertyName("alt image(71 caracteres)")]
    public string AltImage { get; set; }

    [JsonPropertyName("link")]
    public string Link { get; set; }

    [JsonPropertyName("meta-description")]
    public string MetaDescription { get; set; }

    [JsonPropertyName("meta title")]
    public string MetaTitle { get; set; }

    [JsonPropertyName("old_optimization grade")]
    public string OldOptimizationGrade { get; set; }

    [JsonPropertyName("new_optimization grade")]
    public string NewOptimizationGrade { get; set; }

    [JsonPropertyName("Poids")]
    public float Poids { get; set; }

    [JsonPropertyName("Couleur")]
    public string Couleur { get; set; }

    [JsonPropertyName("color_id")]
    public int? ColorId { get; set; }

    [JsonPropertyName("Marque")]
    public string Marque { get; set; }

    [JsonPropertyName("marque_id")]
    public int MarqueId { get; set; }

    [JsonPropertyName("garantie")]
    public string Garantie { get; set; }

    [JsonPropertyName("Stock")]
    public float Stock { get; set; }

    [JsonPropertyName("fabriqué en")]
    public string FabriqueEn { get; set; }

    [JsonPropertyName("est retournable")]
    public string EstRetournable { get; set; }

    [JsonPropertyName("Prix vendeur")]
    public float PrixVendeur { get; set; }

    [JsonPropertyName("Prix brute")]
    public float PrixBrute { get; set; }

    [JsonPropertyName("Prix Promo")]
    public float PrixPromo { get; set; }

    [JsonPropertyName("lien (web et video )\n")]
    public string LienWebEtVideo { get; set; }

    [JsonPropertyName("image principale")]
    public string ImagePrincipale { get; set; }

    [JsonPropertyName("images secondaires")]
    public string ImagesSecondaires { get; set; }

    [JsonPropertyName("seller-id")]
    public string SellerId { get; set; }

    [JsonPropertyName("Created by")]
    public string CreatedBy { get; set; }
}
