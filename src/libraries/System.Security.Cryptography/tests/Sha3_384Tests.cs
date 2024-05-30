// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace System.Security.Cryptography.Tests
{
    public class SHA3_384Tests : HashAlgorithmTestDriver<SHA3_384Tests.Traits>
    {
        public sealed class Traits : IHashTrait
        {
            public static bool IsSupported => SHA3_384.IsSupported;
            public static int HashSizeInBytes => SHA3_384.HashSizeInBytes;
            public static HashAlgorithm Create() => SHA3_384.Create();
        }

        protected override HashAlgorithmName HashAlgorithm => HashAlgorithmName.SHA3_384;

        protected override bool TryHashData(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten)
        {
            return SHA3_384.TryHashData(source, destination, out bytesWritten);
        }

        protected override byte[] HashData(byte[] source) => SHA3_384.HashData(source);

        protected override byte[] HashData(ReadOnlySpan<byte> source) => SHA3_384.HashData(source);

        protected override int HashData(ReadOnlySpan<byte> source, Span<byte> destination) =>
            SHA3_384.HashData(source, destination);

        protected override int HashData(Stream source, Span<byte> destination) =>
            SHA3_384.HashData(source, destination);

        protected override byte[] HashData(Stream source) => SHA3_384.HashData(source);

        protected override ValueTask<int> HashDataAsync(Stream source, Memory<byte> destination, CancellationToken cancellationToken) =>
            SHA3_384.HashDataAsync(source, destination, cancellationToken);

        protected override ValueTask<byte[]> HashDataAsync(Stream source, CancellationToken cancellationToken) =>
            SHA3_384.HashDataAsync(source, cancellationToken);

        [ConditionalFact(nameof(IsSupported))]
        public void SHA3_384_Kats()
        {
            foreach ((string Msg, string MD) kat in Fips202Kats)
            {
                Verify(Convert.FromHexString(kat.Msg), kat.MD);
            }
        }

        [ConditionalFact(nameof(IsSupported))]
        public void SHA3_384_Empty_Stream()
        {
            VerifyRepeating(
                "",
                0,
                "0c63a75b845e4f7d01107d852e4c2485c51a50aaaa94fc61995e71bbee983a2ac3713831264adb47fb6bd1e058d5f004");
        }

        [ConditionalFact(nameof(IsSupported))]
        public async Task SHA3_384_Empty_Stream_Async()
        {
            await VerifyRepeatingAsync(
                "",
                0,
                "0c63a75b845e4f7d01107d852e4c2485c51a50aaaa94fc61995e71bbee983a2ac3713831264adb47fb6bd1e058d5f004");
        }

        [ConditionalFact(nameof(IsSupported))]
        public void SHA3_384_VerifyLargeStream_MultipleOf4096()
        {
            // Verfied with:
            // for _ in {1..1024}; do echo -n "0102030405060708"; done | openssl dgst -sha3-384
            VerifyRepeating(
                "0102030405060708",
                1024,
                "0aa96c328926f2faa796dc75a104e200f5b497beb0313e8822b471efebbb39cef02687e33787883a87c18f35856dcad1");
        }

        [ConditionalFact(nameof(IsSupported))]
        public void SHA3_384_VerifyLargeStream_NotMultipleOf4096()
        {
            // Verfied with:
            // for _ in {1..1025}; do echo -n "0102030405060708"; done | openssl dgst -sha3-384
            VerifyRepeating(
                "0102030405060708",
                1025,
                "e2d1714c8011e7b90550123006128d90fa464cb71903e4aa67342bc8780eb43ae099a2d8610e0ba3061f4f3792d344a7");
        }

        [ConditionalFact(nameof(IsSupported))]
        public async Task SHA3_384_VerifyLargeStream_NotMultipleOf4096_Async()
        {
            // Verfied with:
            // for _ in {1..1025}; do echo -n "0102030405060708"; done | openssl dgst -sha3-384
            await VerifyRepeatingAsync(
                "0102030405060708",
                1025,
                "e2d1714c8011e7b90550123006128d90fa464cb71903e4aa67342bc8780eb43ae099a2d8610e0ba3061f4f3792d344a7");
        }

        [ConditionalFact(nameof(IsSupported))]
        public async Task SHA3_384_VerifyLargeStream_MultipleOf4096_Async()
        {
            // Verfied with:
            // for _ in {1..1024}; do echo -n "0102030405060708"; done | openssl dgst -sha3-384
            await VerifyRepeatingAsync(
                "0102030405060708",
                1024,
                "0aa96c328926f2faa796dc75a104e200f5b497beb0313e8822b471efebbb39cef02687e33787883a87c18f35856dcad1");
        }

        [Fact]
        public void SHA3_384_HashSizes()
        {
            Assert.Equal(384, SHA3_384.HashSizeInBits);
            Assert.Equal(48, SHA3_384.HashSizeInBytes);
        }

        [Fact]
        public void SHA3_384_IsSupported_AgreesWithPlatformVersion()
        {
            Assert.Equal(PlatformDetection.SupportsSha3, SHA3_384.IsSupported);
        }

        private static IEnumerable<(string Msg, string MD)> Fips202Kats
        {
            get
            {
                // Generated from SHA3_384ShortMsg.rsp
                yield return (Msg: "", MD: "0c63a75b845e4f7d01107d852e4c2485c51a50aaaa94fc61995e71bbee983a2ac3713831264adb47fb6bd1e058d5f004");
                yield return (Msg: "80", MD: "7541384852e10ff10d5fb6a7213a4a6c15ccc86d8bc1068ac04f69277142944f4ee50d91fdc56553db06b2f5039c8ab7");
                yield return (Msg: "fb52", MD: "d73a9d0e7f1802352ea54f3e062d3910577bf87edda48101de92a3de957e698b836085f5f10cab1de19fd0c906e48385");
                yield return (Msg: "6ab7d6", MD: "ea12d6d32d69ad2154a57e0e1be481a45add739ee7dd6e2a27e544b6c8b5ad122654bbf95134d567987156295d5e57db");
                yield return (Msg: "11587dcb", MD: "cb6e6ce4a266d438ddd52867f2e183021be50223c7d57f8fdcaa18093a9d0126607df026c025bff40bc314af43fd8a08");
                yield return (Msg: "4d7fc6cae6", MD: "e570d463a010c71b78acd7f9790c78ce946e00cc54dae82bfc3833a10f0d8d35b03cbb4aa2f9ba4b27498807a397cd47");
                yield return (Msg: "5a6659e9f0e7", MD: "21b1f3f63b907f968821185a7fe30b16d47e1d6ee5b9c80be68947854de7a8ef4a03a6b2e4ec96abdd4fa29ab9796f28");
                yield return (Msg: "17510eca2fe11b", MD: "35fba6958b6c68eae8f2b5f5bdf5ebcc565252bc70f983548c2dfd5406f111a0a95b1bb9a639988c8d65da912d2c3ea2");
                yield return (Msg: "c44a2c58c84c393a", MD: "60ad40f964d0edcf19281e415f7389968275ff613199a069c916a0ff7ef65503b740683162a622b913d43a46559e913c");
                yield return (Msg: "a36e5a59043b6333d7", MD: "bd045661663436d07720ff3c8b6f922066dfe244456a56ca46dfb3f7e271116d932107c7b04cc7c60173e08d0c2e107c");
                yield return (Msg: "c0920f2bd1e2d302259b", MD: "3d1584220409f88d38409a29ecaebb490ef884b5acba2c7eaf23914bab7f5f0fc97ee1e6336f88dfd4d0a06e902ccd25");
                yield return (Msg: "70ae731af5e0d92d264ec9", MD: "563359fd93fe09f3fe49fcf5f17e7f92aab589cdec3e55e4c3715e7775814bbbfb8c4c732e28d3b6e6404860812dc6e9");
                yield return (Msg: "69c74a9b0db538eeff64d93d", MD: "88c66389ca2c320a39022aa441fa884fbc6ed2d3cc9ac475372d947d4960579a64e061a297d1831d3524f98d8094404b");
                yield return (Msg: "a4a9327be21b9277e08c40abc7", MD: "751f5da5ff9e2460c99348070d5068d8a3d7ffcec7fd0e6f68f6cd4a2ef4226df8d9b4613c3b0d10a168eaf54eabe01a");
                yield return (Msg: "cc4764d3e295097298f2af8882f6", MD: "10f287f256643ad0dfb5955dd34587882e445cd5ae8da337e7c170fc0c1e48a03fb7a54ec71335113dbdccccc944da41");
                yield return (Msg: "5a23ad0ce89e0fb1df4a95bb2488f0", MD: "23840671e7570a248cf3579c7c8810b5fcc35b975a3a43b506cc67faefa6dbe1c945abc09a903e199f759dcbc7f2c4d0");
                yield return (Msg: "65b27f6c5578a4d5d9f6519c554c3097", MD: "dd734f4987fe1a71455cf9fb1ee8986882c82448827a7880fc90d2043c33b5cbc0ed58b8529e4c6bc3a7288829e0a40d");
                yield return (Msg: "a74847930a03abeea473e1f3dc30b88815", MD: "dba6f929fe55f9d66c5f67c0af3b82f17bcf58b36752f3165c16083fea8fd478ee6903f27f820ad2dd9950afb48c6700");
                yield return (Msg: "6efaf78ed4d293927eef2c3a71930e6e887a", MD: "8218498ab01b63041c2ba0709e3309496124ddf0904543a9e0d9d096a750dda97f7a02208af3d8c618d4be7c2bb2a288");
                yield return (Msg: "fd039eb6e4657388b947ec01e737efbbad47da", MD: "c5b3130ef8dbc580e1103fecae69c9a882d9ebf5a3def5938b07f843452a09c9f72f0dbca91d33b021cf6aa6fe60d2ed");
                yield return (Msg: "9c694943389bdc4e05ad7c2f63ceac2820e1d2d7", MD: "f692c025c5c5f3d1275213c1df9bf9eb6d2188eda90ab5bffe631f1dbf70ebd628caee88b7d149e1ac4e262873979afe");
                yield return (Msg: "0fb18357b018b9bbb2cbb4cac50bc85609c92b8e7f", MD: "d164306c99e3798790f0923fe92dbf2f96c3907127dacaa467c766ac75788062589272cb7690b8af2030dd8bd61a3df2");
                yield return (Msg: "26cb40a460e2e727aeb867e0140d0f34790110deb5d7", MD: "af2a42a4c67c3226c55b89605b0dee27e796c2792115f6097203db5aed89e35f563a8246d399fde00c2a5b97ed5a5e17");
                yield return (Msg: "6690a3a0373c829facc56f824382f4feed6eb184642b4f", MD: "84e1b68bc9e2daefc19b567dec911ef46f5f37a74fdbbb6155e7e646f2735df2ac44e239689eb5b536465dc571e55cb2");
                yield return (Msg: "7d80b160c4b536a3beb79980599344047c5f82a1dfc3eed4", MD: "041cc5861ba334563c61d4ef9710d4896c311c92edbe0d7cd53e803bf2f4eb6057235570770ce87c5520d7ec14198722");
                yield return (Msg: "02128283ffc0cfe254ac8f542be3f05fbe4e855dd22ae98a81", MD: "3840981a766d725f83d334e8982965033a5fbb5107d94ffef33b1f700cd46348091a49f6620c37ae3ef5b20513494826");
                yield return (Msg: "27911dd0a6843ccae965d876aa1916f1dcd71e518f7f2197152e", MD: "f59f8428555984d1526cded8129c649fb1b683d35cec7c5e1209441a6a9e7c17f0784151b5ab8a8c492b402a3acb98c4");
                yield return (Msg: "d9378bb66e8c8dee556d691cbc9fdddd6333ca5d50668862c3c57d", MD: "994532d1a557e990b1cc9e0395a2ad8b05619ca322db9da3c4ed2ee194c051d04582fde72dd2b8f674cf6ec958db75da");
                yield return (Msg: "ae1828047c5f82a7b9712f3399832124b892f2f7aea51c8fe3536cd6", MD: "d51111f8bffb44d81ad19683198f29d2033144d3cd856c749cac5b9cae0e712f500f8d0ef813f38e305ce175a7d6162c");
                yield return (Msg: "7dd2d76fa054cf461e132e9ef914acdc53080a508cdc5368ab8c6224ff", MD: "6c0b3395e4c86518ab0a06267320ee9ec95e50385b7a2527ddaa1bd0ead262c56122d4f4eb08b0ae22b3ee7e6f44dd18");
                yield return (Msg: "6fd72888a021f36e550967cb5605b55b78657c9272d93c3ded340d67da6f", MD: "0551583a5b4007401c77ef4382fd8e245c9cf12e976c9766af6b7ae3c7e07a82b3079f903b083d5ec85cb94e46a85ac0");
                yield return (Msg: "d500eb9546553619cdc31e0848c502db92d547efef3ae5eeaa22258afcf0a9", MD: "5edde2f94f8695f277ec05efcc00761fafd272200aed0e63d221c2b6c65b4972a6526f9a1f2e6ace0e81938f043fe877");
                yield return (Msg: "6189597e0198a18c65fa0bdd0797f13037c75c4058b7d3454c0f71bd2dd13b6c", MD: "110630ca7631b7620e6bee6ed6e929098965571936c34829484983eba9532b8175528c228c57439453f027a4f7c83ca3");
                yield return (Msg: "243b941d748541af303f8e9d2c371cd03e437d62a9df485ddc176dc65da8c7da00", MD: "5884201f7a555ea3c5deeb019fd9e8c161e1b89756045e475b141ec5135ce5a41c93e5e1f79534d36fd8345ba434da43");
                yield return (Msg: "2dc3d789582c1a806c3b491d5972ef8f1733f1f5e02866dc9de2a8029ec0ab608d13", MD: "05a3903b519cdf679120c7ccb4ef178b58e4502fcd461360988fa06669294851e629d9dd3e77ffb73d24599d5d3edd36");
                yield return (Msg: "e5b3f6962fe57230780b3d55b29effe0dfebde2c81ba97d4512ecdbd33eca1576a7f82", MD: "7ac2776afb74f55bbc4f6eccf825ee13ac7445fb54974e6c24ebc0f03fdcd8530199a61106a31b4279e02201ee0f54fd");
                yield return (Msg: "da03486aa3cebbd6502e9f5a6f0f835e973a581befcc1aadefe7b3696ba71c70cd58c584", MD: "02c44ceec0bb7dc0f664ebe44230192b5b0bb646bb944d23fa1ff3586dc0523fa9d7f0dd6df5449ab9edd9a1096b07dc");
                yield return (Msg: "3c686d321ba66185cdca83ba9f41984fa61b826ef56b136e13f1239dadf6e03d877866ccb8", MD: "ad624edd9f2c3a32b56c53d9e813c01d66bcfe424c4a96907d52ac1ddd68370ec86dac67504a90e8a8e75502e01081d2");
                yield return (Msg: "4dcff99fac33840f6532547fb69b456902d6718fd5d4538e23462db6d00da61975f2b8e26298", MD: "cf37dd27997c1bb7e6dc405170066e74c6ce517c029ed8dce126d025da74e0b8e86da567e8d7d8d5b5d3e2a546df7489");
                yield return (Msg: "2799f672328834d7eaef9439795d35ce93c9094f58ded9f17c968a97a50a9e461489fed988e7f6", MD: "85cfc23c97cb13910b808e7033809a45aa0b7f7138de618c2ca622c8b813c988e264af3b96c7925dcbd1d2761757d800");
                yield return (Msg: "c7e947507822f28a562745a8fe6fed6cb47d73145804c894954e21245cde04fa9155a35904926aca", MD: "8bddf3baebbc5b04fe0b0a9c3c2b730abe918ce4892d2843c613ee96da0228512f0d1307c7d1a8922e79a92e957dd18e");
                yield return (Msg: "6c497bf6ff69cb39e3faa349212b8b6691ca237905ac0099c450b6d33abf362bedb65bdeb307bfea23", MD: "3639fab6191b35246278522cfacee0cd5b15580a26c505ae3c46b4b1c2572016b48f1b012bbbedec47916950fbb33a1d");
                yield return (Msg: "d15936f3b0c9018271812b4c81453c4457c7edd110bcea7f5735d6f5882d8f27155eb4cc285a65138ad6", MD: "0293eeef0aa3392c93d9c6ca89c08b317622572d4de2286a4b9ae6c2f9c9e0e64ee6c483d4f10859077e3c6868430214");
                yield return (Msg: "df18139f34b8904ef0681c1b7a3c86653e44b2535d6cecd1a2a17cd5b9357be79b85e5e04dd9eff2ca8b9a", MD: "db9e171d6e3336631c9ceec6b4d732ce62b015939269fb69fae7d22725500e8a2fc9f1459cf0a31fb9d16d7c44583f52");
                yield return (Msg: "0459dcbc149333ea2f937b779a5f3728148449a9aea3662cdd2cc653ce6a2050f9c0d54bf9326c039b263eb9", MD: "464ba409fbb45e985f84ee24662eb7c042c3c2ad9649f1ac4a8b2be9c07d37ed2e4284362057493f6a7e52c356b05bc5");
                yield return (Msg: "eb3f7002c8352270340b8da8643622e5f7e32cdb208a0dec06c6cb9e6b64cc4d8cb9de1d49397b3386464a25d1", MD: "a26bd76ce42d818dbec462d8fe7cdd957e6b84ae8750fb5e1c9c76bc6000e23737e073a59b4600e5056524edc667909d");
                yield return (Msg: "47e3e3d8c68ac9d9f4b3759d8c7d9dd901e35b096ee4c8b6cbe0cdf467463630926c08289abe153bfa1bcde3cd7c", MD: "b504ef475a568f9caba8352a0b2d243acdf3d2b41d8890a6fb3abb8aa28a29e0c7527d20e2d79b25b400ec27c314db72");
                yield return (Msg: "838d9c181c5ab59592723bd69360e0d7fd15232beada7591ea899ac78ffd53a32fc73a5fe522ed35d92a6e2bc148ca", MD: "53e99e1158d59032ffe4b5ea304c7d2f7a61b6b2a96ac97832ca26013549fe3f7dcdf926bd74ceabe4f1ff172daed6e6");
                yield return (Msg: "a90d2aa5b241e1ca9dab5b6dc05c3e2c93fc5a2210a6315d60f9b791b36b560d70e135ef8e7dba9441b74e53dab0606b", MD: "4a16881ce156f45fdfdb45088e3f23be1b4c5a7a6a35315d36c51c75f275733319aca185d4ab33130ffe45f751f1bbc5");
                yield return (Msg: "8c29345d3a091a5d5d71ab8f5a068a5711f7ba00b1830d5ed0bcdfb1bb8b03cd0af5fe78789c7314f289df7eee288735fe", MD: "e27b39a96255ff69c45285fca6edaaa3954ce32c1e3d9b1f60c1b6676594bb45caf0889fc11daf93a1b60746229689dd");
                yield return (Msg: "32876feefe9915a32399083472e3c3805ef261800b25582aa7c36395fd3ec05d47b49c4944bbcc2b8b5ebd081f63ae7943d0", MD: "f96433cdb69a607433ea2eb77d87d3328867dc4076b67ccf17f50f9e08e89a86624b60f2ecdb8affcd431fc13173fe75");
                yield return (Msg: "e2e77eb54f321f86f52ea3d3c8cdc3bc74d8b4f2f334591e5e63b781034da9d7b941d5827037dee40c58dc0d74c00996e582bc", MD: "a352ab33ca730482c376bdc573c9d1dc6d3597f9be9f798b74a57beaa8e9c57b78ee6761056eb67363e882fefcad4fb9");
                yield return (Msg: "da14b6d0b2ec4cf1e7c790e7f8f4212b8f4d05f50e75e2a56a5d70623c0d2e0115a15428129109b3b136d756e38a5c8463304290", MD: "aae7ad977e17ac0e560c0e0186433420f9fddcd191b9e91567cee05df88f1e1aee50424a313998a873f7a9c289a02217");
                yield return (Msg: "2db06f09abaa6a9e942d62741eacd0aa3b60d868bddf8717bef059d23f9efe170f8b5dc3ef87da3df361d4f12bfd720083a7a035e8", MD: "85d4e3e5abcb1b59ca6f551eb43b43ff64890511f73a9083a2ce6e9c2861c6e9664c765629024f4b01b0cd1594a5981b");
                yield return (Msg: "26bad23e51c4560c172076538b28716782ee6304962f68e27182048948d5c367a51a1c206a3e9b25135b40883b2e220f61cb5787ed8f", MD: "a44c7f84ab962f68283404f8c5c4029dbc35d2138e075c9327580baf89f292937bf99422e45756b3f942bf0a5ae4acb6");
                yield return (Msg: "77a9f652a003a83d22fb849b73fed7d37830c0dc53f89cea7dbec24e14f37197765206fe0e6672016e4dec4d9ebbe3e1b4423771a5d0a8", MD: "29c8bb39bb2aad419a00a80216ec71ec5ec9ab54c41927e3e3f2f48f079a5886d7fe89db98c807ab686d2339001d6252");
                yield return (Msg: "268c7b3a84849fec5c769bc4ad377dea10c9d20c91dd17fdbd9670a2fc909d0e212129ec40dee41dbf6194a3b04ae8be5e84ad5426ca4496", MD: "0dfc6ffcf4a387ec09ff862c6139a6f7ac77abb2b5e1f6dc814eb71525f8657ac74a7697c2975c70a543af0e227d03ca");
                yield return (Msg: "b8324341a6891a6b5e001a7d2ebba6e02e8335c124185309a4c9e9907c43bd8d4fa73c527fdf783650316dd24b148870e1436ac05111e9cdcc", MD: "6278d1cc17fb6d54129d04987d4774fa846dcac4ba8b6b72f41e63dc387ce0081ba29fb2c17c6744edae24e669cc9e75");
                yield return (Msg: "5ef8b3d79d299bee2c414560c7de626cc0d9fb429884aa69cc30095ef1f36b7e03a8ca25fb3601189f163b209e0facf8dc447f690b710fb47b72", MD: "7ec9505f33f4a5493574422de078e0490b61be8e8d6f158192bb7d2bdc2dc335598dc88d9b443cd1c14b883a77119df1");
                yield return (Msg: "ad7321c9a8b8f0bfe100811114270daad57f6e88772326b62d88a37a6f55c2cf9f759115ed6a590878e4dcefb592db151538db7de20229d26a181c", MD: "3782d2caa537294e809e9df837b1b07e2f1df07d0f4c12e12459f56eeaa478d5b3a41e519d9414eafa5ddd5661c831ba");
                yield return (Msg: "0719d9664541f0a824f71c83b809bb6afc973c9f7428e1ed11f7c29a558e1698b796aefb49eec2b098faf06bd43e82e1312bf0388c38a5bb523506d3", MD: "362c05f678df92883d56e19221391fb00d0f0afcec51d3e0feb15ba2fb60693b09d69118af649648933259d7b1e240ab");
                yield return (Msg: "5415c2596aa7d21e855be98491bd702357c19f21f46294f98a8aa37b3532ee1541ca35509adbef9d83eb99528ba14ef0bd2998a718da861c3f16fe6971", MD: "8f9fd7d879d6b51ee843e1fbcd40bb67449ae744db9f673e3452f028cb0189d9cb0fef7bdb5c760d63fea0e3ba3dd8d1");
                yield return (Msg: "b979a25a424b1e4c7ea71b6645545248498a2b8c4b568e4c8f3ff6e58d2ac8fbe97be4bea57d796b96041d1514511da5f6351120be7ab428107ef3c66921", MD: "e248a64b6ef112bf3d29948b1c995808e506c049f3906d74c3ee1e4d9f351658681901fe42c8e28024fe31014e2d342b");
                yield return (Msg: "e64c7bb9cd99ce547d43de3cc3b6f7d87a2df9d8a4760c18baf590c740ec53c89bfa075827e1f3f2858ce86f325077725e726103fbe94f7a1466c39f60924f", MD: "d1e5a72d2595f38714c6198ac14f8a5cdd894dcf9b4b8e975174b100df7bbf4f7ce291b4864f27c0b64e6330f6c1c82c");
                yield return (Msg: "91b7a1fd0e20072d9c5be7196e5eaf8df36fdf145895b30d4e4c02010d7c663499ac9d7a44732f4c7430511ba6fb0ae4b3dc9405523a054fdf962f5c5b79c423", MD: "07c2e0aeae30da83b5a6b320aa1cf727b10c2034583d7acda55648fa3daa017aa15588b6e2149101c56e3d7df7c76df1");
                yield return (Msg: "5bbc2d4efe63cbfc9fc221dd8d8384075a79c80a27d6a8c5219e677f4c5bb8338013dc2ab1770acf735d13c0bc704621ec2691350cf3ea2f53bded45ef8fc70702", MD: "dd0bbfe4b799642191abe316df9d59a3743566778b4459c51c3be3f658bdce45516ad188fbe1a8cad8a1fa78f8ebb645");
                yield return (Msg: "129549278e8976c38b5505815725400c3d2081edf141ad002e62ff299d9a0743f9c9f25971710b194dc88285d50b6cec6e140c19072f51cab32a9f6497abd3e407c6", MD: "ca26aec527fadcd5ebeb4eafa7c102f79a3c2edb452afd04f6162dd7a17bdd1aad7d616508a89a3ec6a40791d915acc8");
                yield return (Msg: "b9a9f378adeff4337bc7ec10d526c6dda07028375549f7fda7a81d05662c8a0da3b478f4152af42abb9f9a65c39da095abb8161ba6676b35411234bd466c2914e00370", MD: "99914f684e0b317f9338af0c71e9655a3af7153eb9fabaae61454bf8de9e0bfd274c1eff6c4b550e47afcb3b20fa7d9e");
                yield return (Msg: "101da5b09700dcadf80e5b7900f4e94c54d5f175569a854e488aa36fb41ab7220b0662178ca07a596768528123de3b2a3d944aa412875cedfeaf58dcc6d5b4a033a53b69", MD: "d3e32c9b271e11e4968397d85d76938b974ac1ba55bcbe8d7b7da02dbd7e3b9c9af0d98bbd7e50c436fcf9e3551e3432");
                yield return (Msg: "14761bbc5685b5de692973e2df7c9c4750889c19a952f912c817890546d5e37d940d13a14ac7925abbd875b8cd60e4920896ce6decc8db9f889da2b5489e1d110ff459d885", MD: "272222ed50631aff465c0e6fe49ecdfdca983bcb7231e50903e200b335b845108202c28315912c9c4fd50e2c6f13a9ea");
                yield return (Msg: "ed538009aeaed3284c29a6253702904967e0ea979f0a34a5f3d7b5ab886662da9b8e01efc4188e077c2cdeb5de0a8252aafbee948f86db62aae6e9e74abc89e6f6021a4db140", MD: "8361b680243b1661d6f1df53db363cae41c2ebb7438c00606d76b9c2a253faa1f09d6f520d69d692ec1dca0c7885119c");
                yield return (Msg: "c434d88468f1eda23848d0804b476933f24baeadec69743dd90d8455f1e1f290f6f1aaf3670c4c74f76d3ab83e9bef21ad8d9208c712ca478e70d5fb3c4bd48834c969dd38f484", MD: "9c26e96fcc09a76cc13d24ad25c9cef4300e96e97e4fb59b441baffed07f6a70b1464f2548c7fd7839810dbb9e9c1e18");
                yield return (Msg: "3064e5ba1e7751bf7198e0811ff4d4ca17d1311c25d9c3a316b562691cde75c974b0b52645c134ddcc709d77b6c1bd24cd684265d723c308bb4d0159e6b16d97ed9ceaa57436d302", MD: "1ea779739b204abe911b4923e6f60fece271eedfc7f074fe1919f0cbc6ce2a99234b003389520884b660165f5a1e80f8");
                yield return (Msg: "89d9521ad84b1c9afc2fbd0edc227193acd3330764b0d2cb71bf47c7aac946af85be13858b55976009f3b36b09ced4308052c817c9c4d0295225f61a9659a0874b88667cdcc5213919", MD: "4209bb8f869f6f17c8d5c368c489ac51a75e24a85a12de1b16fefc292ce636ff8fa360e82f05684f6b0b074ba370a933");
                yield return (Msg: "3216662da0227993d88288187177a0287de4eccf245d7c718b8045bbfb8869d93f1fb9e94d7478b0298e628c07e0edaab01dcf79264dc05f8b2181aa3f831dc949726fbcf80de4c9c9ed", MD: "64c45e018cfbc88f8f4ffe3cef0df3a94aab3049fafae28e28efbb2a4b94809eb302caf901010abfa194f72965663d35");
                yield return (Msg: "e776e6749c5b6c7def59cb98340984539280a9874f80412d4df0ee73d58acd1094d49ed4e35125834cf8cfe349e599144e4f2e200aba4fd3eb6d78cde027c1d5620e0270b5e83ab26b8d32", MD: "94bd67b7f2587b0bda5487cc45d00e4365f1ee40073cdf0d23a5ea3fba01eef42a46bfbac5306d67be02d8d918ae5c9a");
                yield return (Msg: "5d8f84b2f208b58a68e88ce8efb543a8404f0ec0c9805c760ad359d13faab84d3f8bb1d2a4bb45e72c0ec9245ffda2e572f94e466cffa44b876d5c5ed914d1ff338e06b74ad1e74d1405d23d", MD: "947350307748c29467f00103d0a07c3c228c5f494fc88fe2352ca5d10449d0dda7076780c05439a09694eb528d1f477a");
                yield return (Msg: "357d5765595065efe281afb8d021d4764fba091adde05e02af0a437051a04a3b8e552ec48fb7152c470412c40e40eec58b842842d8993a5ae1c61eb20de5112321bc97af618bbfbaf8e2a87699", MD: "32286970204c3451958f5155f090448f061dd81b136a14592a3204c6b08e922ee5bb6d6534dbf8efb4bb7387092c8400");
                yield return (Msg: "a8cb78e1485cbb7a9474c1c1f8e0f307cda5139a7e947df5ea20ac330a6dffcad4a9bd755f9f58724789eeee532615be550dd84f5241fde0e3058aeedbf287f02a460445027f5e6b3829bf71ecf4", MD: "51168bfeef8a981c0def0c4cb067baf15ce5feb8d5f7e9d6076b2836267391aee1fd3a0b5d3434ceb5cf2d6fa06fa063");
                yield return (Msg: "81acca82545e767ab59dcc750a09849cebad08ff31c9297f4fd510ebe6c27769938319180ccc66f36b1a7cf9c9f3538b0f6f371509f77cf0bc4d6d87facc85b933f2e27f8e1bf6cf388f80c0fcbfba", MD: "4ae44d6509986893a8414753b57d11f9c554d89c15ad6d70687c56c6c2ac73537acbb0d51f48e6bea6cf762d58890d7a");
                yield return (Msg: "94987498b1ca87a6f3fa4b999db726115c455d0ec24029b2f5810e49a94668864b8c470f7fc07c3dcd97f41c973b45ba4fa7879ee7546596881573b6863fc39d940eb3fa3444084f721341f5d23d2561", MD: "a733b118be72a187ddcbe5ba67e04b589f9cd9f8482c4bd9d64c580aba7d19d2d1f9c1ddf95fe6efdeffd44f67fcabb5");
                yield return (Msg: "de6b32c2d40d0659166db235259b530ea43f44e75d8b3e9e856ec4c1410bbea3696964af8b6c5dfd3304282369a4bc4e7cf66b91fecd0c7c105b59f1e0a496336f327440980a34614ee00fff2587d6b813", MD: "17ba30c0b5fc185b3245313b83dd0481145953101128914765784af751745b8a2b6a90a434548f3adaf1f07f18649890");
                yield return (Msg: "854211bedacc19f77b46cfa447a4ad672ea9b643f09f5cf5274ba28888207e2466b38127776fb976db8ad7165a378df6ee1e3a0f8109c9aff7e0d6126fd71333c6e6ebe15d7a65151d6a4a83b82c8a6f3149", MD: "ca85632a9f7c32ac4705c6458770025dda4fd07a8d5d6921b897b0da490d64400587649f2d20bf608b9a18d071b63b48");
                yield return (Msg: "822373d9d3d5b06a8da48a43095740fb98c9caf717350fd2c3b058024ff705b9346b7f0a495a6d4d93802bc45ece777f8c6a6e7c2ef6b8135115ff911a2ba5241665b6f7cbfa1b9d93b011b3aaa1dac1853fb2", MD: "6e84587c8c6e54353a6032e7505902ef7f0f0538dd1bb32922e13a7d4d98c47a541015381eab27e9186398120da7fb32");
                yield return (Msg: "c04b701f688092bbd1cf4217bc4b5877f2e60c087bdac46611482a61d51f820140403bc85be0c336332da0938734bde8c502014f3509266c73c6c93c22a1bd0ddf15a5ce7410c2894e9d092e32c079922ba1abb7", MD: "75c585503f15a526113608bc183180b1cb80f4d1b466c576bf021b1ce7a1528391f70e10446681849fa8a643cb2b6828");
                yield return (Msg: "009dd821cbed1235880fe647e191fe6f6555fdc98b8aad0ff3da5a6df0e5799044ef8e012ad54cb19a46fdd5c82f24f3ee77613d4bed961f6b7f4814aaac48bdf43c9234ce2e759e9af2f4ff16d86d5327c978dad5", MD: "02a09d37d31e4365c26bec0eaacecf29eea4e8d21ab915dd605248764d964f10ebb8fafdb591982d33869a1d08a7e313");
                yield return (Msg: "0b7dd6709d55e0d526d64c0c5af40acf595be353d705be7b7a0b1c4c83bbe6a1b1ec681f628e9d6cfc85ad9c8bb8b4ecac64c5b3a9b72f95e59afefa7bcec5be223a9b2b54836424afde52a29b22ab652d22cce34b39", MD: "5c84ae39d959b79555231746ad5b33689a31720ed0070f6772147977edd0aead07fb8b7b71b0bd587ebc5c1a80d564c7");
                yield return (Msg: "3e9b65d7bf4239420afa8639c8195b63902b24495b95c4143978e49843d88a92d1feed2eed1a88cd072d6d04ea26dce8ee4b14896fdb69bc7ff2971ed8ac5655148d2e9921218d74efdf17c56b533d0bb17d11e07d7458", MD: "ab7890d1b51af10285752bf9da5eee5c3e87a285dc33262d0261aa9a575f303e94845d7ab21b48f4e6884568cd78b550");
                yield return (Msg: "9436da433d1ebd10b946b129cb34bccec9b8f705aaba3f8561352ed36a8449aba2dd7ba15b1bc308b0c02913163af63a346524dff5521432db477f529606afb5d552efc95cb040db566b4d39eddaa19319e518a7b5c6931e", MD: "968ae9104f9c907c5a72936250dfedd62cd04f6e5ddd2c113490808a11884449aaef5d013ea3993a6cb6fc5c08754408");
                yield return (Msg: "37254bf9bc7cd4ed72e72b6bb623a0cc8eeb963d827aef65ad4bc54913235b6d3551533ce33421aa52ffbf186eb9a2787188eeb1b52ee645c6d4a631bc071415c80014940c28fbfeb0db472c326c8dacfd6ab21f3e225edef3", MD: "975e10fac9aa77b780e5f6c2151ec4a3c72ff26e41233cc774c074df1b78cce5af1191ba955a0bce15926ae691b0ffe7");
                yield return (Msg: "79e77cd08a6ef770bbe4bedf61557ea632b42d78637149670d4d6157d56ed7b2ccaee45d9439dcebc557b4118e86c15aa0ccc21c474b21abda1676cc56434d6d46422993e66dc99387dfa985358accf69884b9dd18a2c4d04448", MD: "94729f5f99a54f5a3ea69233ff9d522392d4596eb6ac2bbb07492ece3c67317412bb47ae317ddd20536c3adc003862f1");
                yield return (Msg: "64b76cb554f6becc238a3fcfc3eb97993667ec82fdc3fb28d42567709c3250c7997328aeddfdc2750451ac462281bf66fa94f4b8712c7a8342660574f20268e707c466627519c56259fea55be91e10faab3ad2ade6ce8b6557f202", MD: "26d48ef5067d704ee9e2a64e399de23068908b3c911ffc4056c168362c37385c92d37d51354b6505a82c4d22fec37eaa");
                yield return (Msg: "3df27829bfb1ab7d381f146b30370ef56b392b73b35b1be5d8bbcf88f499dda7f3c327b45350b8972991ee466545de96560cf451711fda884e3d9b2af3e909d655d25cee1c931beda79c40fa507097bdf1126771a7b9543ad5cb84b9", MD: "5fa4ebfa24150236c03409f0857b31cb95b0150f381c8858b01559957b1268f73c698709233e6b15468675a102d0c5e5");
                yield return (Msg: "b00f4e67ca08ccfa32b2698f70411d8f570f69c896e18ec8896cfe89551810543303f7df0c49f5b94783cce7df8d76d0b88d155633302d46003711f233339b1c9a8c20164ec8a328890a4932b7d90d92d023b548e4820558f8bd327010", MD: "eaa756b5892fdfc793d74e3f9f4d6c7a5a6a2241dd11e0c38ced59c8ec7be377a41d1d06774a5970ce9722d8e119d0ad");
                yield return (Msg: "a4f95f6a46a9cbf384a7e98e102d1fdc96839d1bf26b35a5a0bd6cb9734fd17e8a178d4581943c0fe469fb4fe94cc2f15e1ef59ae05b35324eb57ca07dfc69d42d41d80b3c3bb64e1aea143c7d79790a56697dc803ec93e6c68f27f6761c", MD: "1aff8d9c64f0c162ed0195d1f3a342a010d14be0636903c48020ba42de1cfa8b98ae2142d89af3e69e9eb4c735857dd1");
                yield return (Msg: "02713084bf93fdc35135515243c3bc0f4b2b447f2d3461c0dc104cbfe23479ab036762a91d1987c953f7b3386abc80b8734a1d4eabf94f3a9f2fb62c943152b5253846fc2ec8dbb2e93dc74857a7b05fe2d7ec8040ba8b0d9ae69777ee739a", MD: "84da02114e341a3636f00822b32bd21a8a1f7b39f2956bd97f39346fedf9aae63b304c65c93a541e8bcda549576d5f27");
                yield return (Msg: "00ce225eaea24843406fa42cc8450e66f76ac9f549b8591f7d40942f4833fc734a034c8741c551d57ddafb5d94ceb4b25680f045038306e6bcc53e88386e2b45b80b3ba23dec8c13f8ca01c202ae968c4d0df04cdb38395d2df42a5aff646928", MD: "81d6e0d96575a9b8ca083ee9ec2ead57ddf72b97d7709086a2f4a749d3f61d16423463487562c7f09aba1b26e8cae47b");
                yield return (Msg: "7af3feed9b0f6e9408e8c0397c9bb671d0f3f80926d2f48f68d2e814f12b3d3189d8174897f52a0c926ccf44b9d057cc04899fdc5a32e48c043fd99862e3f761dc3115351c8138d07a15ac23b8fc5454f0373e05ca1b7ad9f2f62d34caf5e1435c", MD: "00e95f4e8a32a03e0a3afba0fd62c7c3c7120b41e297a7ff14958c0bdf015a478f7bab9a22082bfb0d206e88f4685117");
                yield return (Msg: "2eae76f4e7f48d36cd83607813ce6bd9ab0ecf846ad999df67f64706a4708977f0e9440f0b31dc350c17b355007fed90d4b577b175014763357ce5a271212a70702747c98f8f0ad89bf95d6b7fbb10a51f34d8f2835e974038a3dd6df3f2affb7811", MD: "eb396cfaf26ee2775af3c9a3a3047664ca34cbc228ccbb966df187d518717df6a328ecc316ed0ed09b170080eccc486f");
                yield return (Msg: "093e56d33bd9337ad2ad268d14bac69a64a8a7361350cf9f787e69a043f5beb50eb460703578a81be882639f7e9ac9a50c54affa3792fd38464a61a37c8a4551a4b9ff8eed1f487ef8a8f00430e4d0e35a53ff236ce049b7a3abdc5cd00b45c4f3d49b", MD: "4a339128486e5b274fc4ed538c0ec9e57f780e9c500c5f92b04ae81a22fbeebf3785259a0bb3b6d9b47f31873cd8dffa");
                yield return (Msg: "0593babe7a6202077c026e253cb4c60ee7bad7b1c31a20da7aa0ce56b622eb57ed07d21a7f0ae6c6fe3c8398cc48353decfb287f1204e024fcf82a13059953b9f85797ab2217dc8dab34a13226c33104661c1ca79396e7d97e91039d32bafc98cc8af3bb", MD: "5981815c1618cc49cd5cf71a4b7b32b8cd7b7ef553bfaef2149ac723ff2582a2d345c5bd05943e155ced1e5f091c5601");
                yield return (Msg: "ae1828047c5f82a7b9712f3399832124b892f2f7aea51c8fe3536cd6a584b4a7777cc1ecac158c03354bb467b8fe2c8ce2f4310afd1e80fec51cc5ad7702566b2c5d21bc6571e4b8e7c59cb4c9e23f1ecb57ada9e900e4aa308874c2d12d34be74c332bbce", MD: "7257f5bfa7d33d1cf5f4550d0cb78750e84c5b7d25027da6acec64bdf30879a0e5c97fe7c468e743aa5ec2bddb29d193");
                yield return (Msg: "3bceedf5df8fe699871decb7dd48203e2518fb0fce0f865f46adce5c133a921320bf40915456204869a3ceb5fca3ed40e0a41a64b8951f0fc580694cfc55bd1f5ce926b07e3e32ac6e055de9b961ce49c7ee41e06b024559b933a79518192e969855889c85d1", MD: "60d7f8bd85fb7a13701db5aded2b7771ab5e476ec34f1fd4298978defbd2b31bb2979391559a164b3ed28f6a39031a11");
                yield return (Msg: "6c36147652e71b560becbca1e7656c81b4f70bece26321d5e55e67a3db9d89e26f2f2a38fd0f289bf7fa22c2877e38d9755412794cef24d7b855303c332e0cb5e01aa50bb74844f5e345108d6811d5010978038b699ffaa370de8473f0cda38b89a28ed6cabaf6", MD: "b1319192df11faa00d3c4b068becc8f1ba3b00e0d1ff1f93c11a3663522fdb92ab3cca389634687c632e0a4b5a26ce92");
                yield return (Msg: "92c41d34bd249c182ad4e18e3b856770766f1757209675020d4c1cf7b6f7686c8c1472678c7c412514e63eb9f5aee9f5c9d5cb8d8748ab7a5465059d9cbbb8a56211ff32d4aaa23a23c86ead916fe254cc6b2bff7a9553df1551b531f95bb41cbbc4acddbd372921", MD: "71307eec1355f73e5b726ed9efa1129086af81364e30a291f684dfade693cc4bc3d6ffcb7f3b4012a21976ff9edcab61");

                // First 5 from SHA3_384LongMsg.rsp
                yield return (
                    Msg:
                        "5fe35923b4e0af7dd24971812a58425519850a506dfa9b0d254795be785786c319a2567cbaa5e35bcf8fe83d943e23fa5169b73adc1fcf8b607084b15e6a013df147e46256e4e803ab75c110f77848136be7d806e8b2f868c16c3a90c14463407038cb7d9285079e" +
                        "f162c6a45cedf9c9f066375c969b5fcbcda37f02aacff4f31cded3767570885426bebd9eca877e44674e9ae2f0c24cdd0e7e1aaf1ff2fe7f80a1c4f5078eb34cd4f06fa94a2d1eab5806ca43fd0f06c60b63d5402b95c70c21ea65a151c5cfaf8262a46be3c72226" +
                        "4b",
                    MD: "3054d249f916a6039b2a9c3ebec1418791a0608a170e6d36486035e5f92635eaba98072a85373cb54e2ae3f982ce132b");

                yield return (
                    Msg:
                        "035adcb639e5f28bb5c88658f45c1ce0be16e7dafe083b98d0ab45e8dcdbfa38e3234dfd973ba555b0cf8eea3c82ae1a3633fc565b7f2cc839876d3989f35731be371f60de140e3c916231ec780e5165bf5f25d3f67dc73a1c33655dfdf439dfbf1cbba8b779158a" +
                        "810ad7244f06ec078120cd18760af436a238941ce1e687880b5c879dc971a285a74ee85c6a746749a30159ee842e9b03f31d613dddd22975cd7fed06bd049d772cb6cc5a705faa734e87321dc8f2a4ea366a368a98bf06ee2b0b54ac3a3aeea637caebe70ad09ccd" +
                        "a93cc06de95df73394a87ac9bbb5083a4d8a2458e91c7d5bf113aecae0ce279fdda76ba690787d26345e94c3edbc16a35c83c4d071b132dd81187bcd9961323011509c8f644a1c0a3f14ee40d7dd186f807f9edc7c02f6761061bbb6dd91a6c96ec0b9f10edbbd29" +
                        "dc52",
                    MD: "02535d86cc7518484a2a238c921b739b1704a50370a2924abf39958c5976e658dc5e87440063112459bddb40308b1c70");

                yield return (
                    Msg:
                        "7f25b2c0eb1a6911cc3328fcdcd40f28f010375f7b1b51a05402896fb999b17093b59b34fb9cc653feba3dbb9d96bd47180416946d9bd3101b691d532be6ddb3712721121054c1fb3c5c42ee44e7faf7cf8d75856545187a3220047f07373e9aa2e10c022f2aa232" +
                        "0f81fd3cd7b110609c131edd6e016707228d069a55731a4ead4d24ab6206b01ffd91384e60db45a907fed7428db707de721aeb4c1b84baf61ad230b6b0d034eb90f4b9cbe64de2fb98b6695dcc4f4129aa2e7a3f635166bb72d7faca227076bd5013495c72ef2e7d" +
                        "d8a39cd532b15d0d53307c1834c265c53cc64890becfbebec454afa90ba973584e2d3752c7c6a3b4f48aba8297bf013b0006e3b08ed354157420b559b963f7b383bd047e94745a4615a3f9239230804547ff93d19a657fece8e02114840504b7fdb9c9fea0a4ccea" +
                        "3ee304a330fd2b0d97191f9be86e8968a9fabc847577e08b468b4f7df43f3fc9f8b2a2ab760f4ab87bbc51b883d4b8b33ed84e4f93a1d359e6995ea1962bfc0bca789ae36e4c25717850efcd708155f52fe09f1de76b2746634dbe1290524bd73d9db5f21f9d035e" +
                        "183dc2",
                    MD: "927962c873a69caa05cadc1cb485eb1cbb07748e47d942192df4af9233f42b95a638918306ae83a8237d21c2824f666d");

                yield return (
                    Msg:
                        "386f98670b177683d0b804c5875fe9c7afa233ee66349c9fd1b60bb0becf5e1d887e67fd3baf34b4f90d94699d18d6bb9d77d4af358f31edc254de2d6c5fe3ec07425c633b18c1b9e3606b78b40b543e1fd31fb578cf58c45744fc073fbf3c7d7d607e815379a5fc" +
                        "565892d81560eab8fb5f1ae6771b998c592e6d288014f13ab283d53fcbfa66e31a9d107308402191fac2cf2b799c7dae91b93a7676898b8a6e516a86eac58ed8f6d8ed2fd4d38031e4a4466dc8798b90c48e6adb6b4391d47872443cfaffa542b4b132f6c3408f00" +
                        "81af8692aadb4c9bbd55053ea56d8b82998f6b4b41d331891acfe6af1bb0d6679989978368ea463743b514866d2d01fb9950e8990867bc14f1db1142254adeccf3da812949cd03cd1d569e9d0bab7ca7405cc21096e3cd4d007cbb9629372e98584b4c6b97ad0bc3" +
                        "14e1ab6ac71184ee555c01973570ed9b115bed956f9e4e349083013098b1e483f0fe44d5e9849f38a2f7ae152b36a266ea1faf263ea8c706632ba8629602187379546fc6b82e57ededd6d074c15c771754710731e07c207899eb47e8d7c72ffd768c36257d373375" +
                        "ffa06f9b3f0af11417f9ff9f9b44e1f1f96ae8aaa429af88b14da1da81c7bb38a0fe9372ed6a9ac6fb5e9e56b82593d94c5192904450227bf040b7ce0904789f979845e112a1f995c849ec3f7e49bd975a474e8201630f40fc0d80e76019f110ae158cd0f8da96ea" +
                        "4561f242",
                    MD: "d30ec9a7baeabe40f6648a624dddf8721c89542e258f0fa9afcc9e68433faef781824048b0b771a94e8f0c17a403f9fb");

                yield return (
                    Msg:
                        "8c569727f1d4548f1c66a5c830346259612d10c5fef90518ae2fcdbffac9cd9c0bd5265ab56ddcfeb5e838bf37526a189c1a731b790b4208e37d1d1eeacd43b1630ad07debf1e03a281cf7715276a18df2f25535ea7d9fd9b6317f8bf1cb0c111b5f5c38994aa86b" +
                        "fd69ac8388884de1ed1d7eba583764b3afb1b8ae18ab6ee3bb3a9432c95f7cb7bd361da0e270b73b1503b653cc20d9bd5766932e6655b250cc053e148218a449efed136e661627c4f10dc5a84d22462035b8d7b4e4b11f7fd5272385cd5d67471bf556951e63e4a4" +
                        "09a17260e324f203d2104be798a8ff985e080b2eb1160fdacab6aebe123d3802e5298624960f268fb4d4b9708c2098c5ef10beb6362be2298298e391498e69060e0bca9b6fc92aec656ee7f6c802342c11a703c76484295dce03bcb5cb3cc0da0bb1036e753b4641" +
                        "6d449d22523719f54b35a306440a2b9d335f03d3a03085a36481fc44b14dc2b652c0a59c34a68f492622671ddda332123b147e92d153008ca2e57cc629e8e5759e48c60b7636e05029d614b4373884e36d8af69b648c79ba4c444a9ce7f2f8a3d846c7171ed15231" +
                        "dcba75725bb26a395129329564c23758ea052f6df355436b89217169365e2f15c734510050f72c3c705afc29d6df838c0492f3e153f70ef338418ca9c5c4bd2373ad6f051ef1121351831affc4caa57e23525ea111c2a1636d0ee07fd4ed4584678e982ace8664e7" +
                        "7d0e55be356be558cead3755359c43e4b1f034916ac00e5f2b3d941767a069df7a61750e32aa8a3f8e0b48a5c56f3e9e8a4f518a8f2562dd48242b73f1266a24d2e64299c26fde5dead45737cb22d8b8839300104b04872645a925e77500afdd0c038404eda227da" +
                        "6a702db64e",
                    MD: "91e24f999cac1b9ab9ae456ecf47b52c1144ffd1df2d95feb05fce930e37ff767a005cf07bb7af45c8a73585e8544965");
            }
        }
    }
}
