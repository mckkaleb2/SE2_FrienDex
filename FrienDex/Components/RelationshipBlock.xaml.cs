using FrienDex.Data.Entities;

namespace FrienDex.Components;

public partial class RelationshipBlock : ContentView
{
    public static readonly BindableProperty RelationshipNameProperty =
        BindableProperty.Create(
            nameof(RelationshipName),
            typeof(string),
            typeof(RelationshipBlock),
            string.Empty);

    public string RelationshipName
    {
        get => (string)GetValue(RelationshipNameProperty);
        set => SetValue(RelationshipNameProperty, value);
    }

    public static readonly BindableProperty RelationshipDescriptionProperty =
        BindableProperty.Create(
            nameof(RelationshipDescription),
            typeof(string),
            typeof(RelationshipBlock),
            null);

    public string? RelationshipDescription
    {
        get => (string?)GetValue(RelationshipDescriptionProperty);
        set => SetValue(RelationshipDescriptionProperty, value);
    }

    public static readonly BindableProperty RelatedPersonProperty =
        BindableProperty.Create(
            nameof(RelatedPerson),
            typeof(Person),
            typeof(RelationshipBlock),
            null);

    public Person RelatedPerson
    {
        get => (Person)GetValue(RelatedPersonProperty);
        set => SetValue(RelatedPersonProperty, value);
    }
    public RelationshipBlock()
	{
		InitializeComponent();
	}
}