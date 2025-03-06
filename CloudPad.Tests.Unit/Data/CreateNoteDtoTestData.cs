using System.Collections;
using CloudPad.Core.Dtos;

namespace NoteTakingApp.Tests.Unit.Data;

public class CreateNoteDtoTestData:IEnumerable<object[]>
{
    private readonly IEnumerable<object> _testData =  new List<CreateNoteDto>
        {
            new CreateNoteDto
            {
                Title = "Project Kickoff Notes",
                Content = "Initial planning for the CloudPad project. Key points:\n- Timeline: 3 months\n- Team: 4 developers, 1 designer\n- Priority features: note creation, tagging, search",
                Tags = new List<int> { 1, 2, 6 },  // Work, Meeting, Projects
                CategoryId = Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7"),  // Business
                IsFavorite = true
            },
            
            new CreateNoteDto
            {
                Title = "Weekly Grocery List",
                Content = "- Vegetables: carrots, spinach, bell peppers\n- Fruits: apples, bananas\n- Dairy: milk, yogurt\n- Protein: chicken, tofu\n- Grains: rice, pasta",
                Tags = new List<int> { 3, 4 },  // Personal, Shopping
                CategoryId = Guid.Parse("f5a12e8a-c39d-4c0c-8721-0123456789ab"),  // Home
                IsFavorite = false
            },
            
            new CreateNoteDto
            {
                Title = "Feature Development Plan",
                Content = "CloudPad feature roadmap:\n1. Core note functionality\n2. Tag and category implementation\n3. Search and filter capabilities\n4. Cloud sync\n5. Mobile app development",
                Tags = new List<int> { 5, 6 },  // Ideas, Projects
                CategoryId = Guid.Parse("d3d7b4c6-e8e8-4a4e-a46a-3579f2268c67"),  // Development
                IsFavorite = true
            },
            
            new CreateNoteDto
            {
                Title = "Programming Resources",
                Content = "Useful resources for C# development:\n- Microsoft Docs: https://docs.microsoft.com/en-us/dotnet/csharp/\n- ASP.NET Core tutorials\n- Entity Framework Core documentation",
                Tags = null,  // Books, Learning
                CategoryId = null,  // Education
                IsFavorite = false
            },
            
            new CreateNoteDto
            {
                Title = "Summer Vacation Ideas",
                Content = "Potential destinations:\n1. Barcelona, Spain\n   - Sagrada Familia\n   - Beach activities\n2. Kyoto, Japan\n   - Traditional temples\n   - Cherry blossom season\n3. New Zealand\n   - Hiking and nature",
                Tags = [],  // Travel, Personal
                CategoryId = Guid.Parse("a147cba7-36c9-4b5a-8c2e-86e345f7e5c2"),  // Travel
                IsFavorite = true
            },
            
            new CreateNoteDto
            {
                Title = "Homemade Pizza Recipe",
                Content = null,
                Tags = new List<int> { 10, 11 },  // Recipes, Food
                CategoryId = Guid.Parse("b9b4e8d6-7e9c-40d2-a2df-2c8f634b6f44"),  // Cooking
                IsFavorite = false
            }
        };
    public IEnumerator<object[]> GetEnumerator() 
    {
       foreach(var data in _testData)
       {
           yield return [data];
       }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}