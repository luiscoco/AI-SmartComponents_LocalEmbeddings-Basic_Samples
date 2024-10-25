using SmartComponents.LocalEmbeddings;

//https://github.com/dotnet/smartcomponents/blob/main/docs/local-embeddings.md

//----------------------------------------------------------------------------------------
//
//                 EXAMPLE 1 - Assess their Semantic Similarity:
//
//                 As you can see, "Kittens!!!" is:
//                 ... perfectly related to itself
//                 ... fairly related to the statement about cats
//                 ... less related to the statement about dogs
//                 ... very unrelated to the statement about snooker
//
//----------------------------------------------------------------------------------------

//using var embedder = new LocalEmbedder();
//var cat = embedder.Embed("Cats can be blue");
//var dog = embedder.Embed("Dogs can be red");
//var snooker = embedder.Embed("Snooker world champion Stephen Hendry");

//var kitten = embedder.Embed("Kittens!!!");
//Console.WriteLine(kitten.Similarity(kitten));  // 1.00
//Console.WriteLine(kitten.Similarity(cat));     // 0.65
//Console.WriteLine(kitten.Similarity(dog));     // 0.53
//Console.WriteLine(kitten.Similarity(snooker)); // 0.37

//----------------------------------------------------------------------------------------
//
//                 EXAMPLE 2 - Similarity Search:
//
//                 You can find the closest matches from a set of candidate embeddings
//
//----------------------------------------------------------------------------------------

//using var embedder = new LocalEmbedder();

//var sportNames = new[] { "Soccer", "Tennis", "Swimming", "Horse riding", "Golf", "Gymnastics" };

//var sports = sportNames.Select(name => new Sport
//{
//    Name = name,
//    Embedding = embedder.Embed(name)
//}).ToArray();

////You can find the closest 3 Sport instances for the string "ball game"
//var candidates1 = sports.Select(a => (a, a.Embedding));
//var target = embedder.Embed("ball game");
//Sport[] closest1 = LocalEmbedder.FindClosest(target, candidates1, maxResults: 3);

//Console.WriteLine(embedder.Embed("ball game").ToString());
//Console.WriteLine(string.Join(", ", closest1.Select(x => x.Name)));

////Another shorter alternative is using this command:
//var candidates2 = embedder.EmbedRange(sports, x => x.Name);
//Sport[] closest2 = LocalEmbedder.FindClosest(
//  embedder.Embed("ball game"),
//  candidates2,
//  maxResults: 3);

//Console.WriteLine(string.Join(", ", closest2.Select(x => x.Name)));


//class Sport
//{
//    public string? Name { get; init; }
//    public EmbeddingF32 Embedding { get; init; }
//}

//----------------------------------------------------------------------------------------
//
//                 EXAMPLE 3 - Embedding in a chosen format:
//
//                 A common technique for reducing the space needed to store vector data is quantization
//                 There are many forms of quantization
//                 LocalEmbeddings has three built-in storage formats for embeddings, offering different quantizations:
//                 EmbeddingF32, EmbeddingI8 and EmbeddingI1
//
//----------------------------------------------------------------------------------------

//using var embedder = new LocalEmbedder();

////CASE: Using EmbeddingI1
////
////Each component is stored as a single bit, equivalent to LSH quantization in Faiss
////This is a massive reduction in storage, at the cost of moderate reduction in accuracy
////

//// To produce a single embedding:
//var embedding = embedder.Embed<EmbeddingI1>("Hello World!");

//var sportNames = new[] { "Soccer", "Tennis", "Swimming", "Horse riding", "Golf", "Gymnastics" };

//var sports = sportNames.Select(name => new Sport
//{
//    Name = name,
//    Embedding = embedder.Embed<EmbeddingI1>(name)
//}).ToArray();

//// Or to produce a set of (item, embedding) pairs:
//var candidates = embedder.EmbedRange<Sport, EmbeddingI1>(sports, x => x.Name);

//class Sport
//{
//    public string? Name { get; init; }
//    public EmbeddingI1 Embedding { get; init; }
//}

//----------------------------------------------------------------------------------------
//
//                 EXAMPLE 4 - Persisting embeddings:
//
//                 
//----------------------------------------------------------------------------------------

using var embedder = new LocalEmbedder();

// Normally you'd store embeddings in a database, not a file on disk,
// but for simplicity let's use a file
var originalEmbedding = embedder.Embed<EmbeddingF32>("The chickens are here to see you");
using (var file = File.OpenWrite("somefile.txt"))
{
    await file.WriteAsync(originalEmbedding.Buffer);
}

// Now load it back from disk. Be sure to use the same embedding type.
var loadedBuffer = File.ReadAllBytes("somefile.txt");
var loadedEmbedding = new EmbeddingF32(loadedBuffer);

// Displays "1" (the embeddings are identical)
Console.WriteLine(originalEmbedding.Similarity(loadedEmbedding));

Console.WriteLine(originalEmbedding.(loadedEmbedding));