using System.ComponentModel.DataAnnotations;

namespace Stratis.STOPlatform.Models.SetupViewModels
{
    public enum TokenType : uint
    {
        [Display(Name = "Standard Token(SRC20)")]
        StandardToken,

        [Display(Name = "Dividend Token")]
        DividendToken,

        [Display(Name = "Non Fungible Token(SRC721)")]
        NonFungibleToken
    }
}
