using System;
using System.Collections.Generic;
using System.Text;
//using static Android.Provider.Contacts;
//using static Android.Util.EventLogTags;
//using static Java.Util.Jar.Attributes;

namespace FrienDex.Data.Entities
{
    public abstract class Block
    {
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the type of the block, which determines its behavior and characteristics within the system.
        /// </summary>
        /// <remarks>The block type influences how the block interacts with other components and may
        /// affect rendering or processing logic.</remarks>
        public required BlockType Type { get; set; }

        public override string ToString()
        {
            string formatter =
                $"Block - {Id}"
                + $"\n\tB- Type: {Type}";
            return formatter;
            //return base.ToString();
        }

    }
    public class TextBlock : Block
    {
        /// <summary>
        /// Gets or sets the content of the text block, which contains plain text notes created by the user.
        /// </summary>
        public required string Content { get; set; }

        public TextBlock()
        {
            Type = BlockType.TextBlock;
        }

        public override string ToString()
        {
            string formatter =
                $"Block - {Id}"
                + $"\n\tBtxt- Type: {Type}"
                + $"\n\tBtxt- Content: {Content}";
            return formatter;
            //return base.ToString();
        }
    } // end of TextBlock class
    public class ImageBlock : Block
    {
        /// <summary>
        /// Gets or sets the URL of the image associated with this block, which points to the location of the image resource.
        /// </summary>
        public required string ImageUrl { get; set; }
        public ImageBlock()
        {
            Type = BlockType.ImageBlock;
        }
        public override string ToString()
        {
            string formatter =
                $"Block - {Id}"
                + $"\n\tBimg- Type: {Type}"
                + $"\n\tBimg- ImageUrl: {ImageUrl}";
            return formatter;
            //return base.ToString();
        }

    } // end of ImageBlock class
    public class DatePickerBlock : Block
    {
        /// <summary>
        /// Gets or sets the selected date for this block, which allows users to choose a specific date relevant to the context of the block.
        /// </summary>
        public required DateTime SelectedDate { get; set; }
        public required string DateDescription { get; set; }
        public DatePickerBlock()
        {
            Type = BlockType.DatePickerBlock;
        }
        public override string ToString()
        {
            string formatter =
                $"Block - {Id}"
                + $"\n\tBdte- Type: {Type}"
                + $"\n\tBdte- SelectedDate: {SelectedDate}";
            return formatter;
            //return base.ToString();
        }

    } // end of DatePickerBlock class
    public class EventBlock : Block
    {
        /// <summary>
        /// Gets or sets the name of the event associated with this block, which provides a descriptive title for the event.
        /// </summary>
        public required string EventName { get; set; }
        /// <summary>
        /// Gets or sets the date and time at which the event occurs.
        /// </summary>
        /// <remarks>This property is required and must be set to a valid <see cref="DateTime"/> value
        /// representing when the event takes place.</remarks>
        public required DateTime EventDate { get; set; }
        /// <summary>
        /// Gets or sets the comments associated with the event.
        /// </summary>
        /// <remarks>This property can be used to provide additional context or notes regarding the event.
        /// It may contain any relevant information that helps in understanding the event's significance.</remarks>
        public string? EventComments { get; set; }
        public EventBlock()
        {
            Type = BlockType.EventBlock;
        }

        public override string ToString()
        {
            string formatter =
                $"Block - {Id}"
                + $"\n\tBevt- Type: {Type}"
                + $"\n\tBevt- EventName: {EventName}"
                + $"\n\tBevt- EventDate: {EventDate}"
                + $"\n\tB- EventComments: {EventComments}";
            return formatter;
            //return base.ToString();
        }

    }
    public class RelationshipBlock : Block
    {
        /// <summary>
        /// Gets or sets the name of the relationship, which describes the nature of the connection between entities.
        /// </summary>
        public required string RelationshipName { get; set; }
        /// <summary>
        /// Gets or sets the description of the relationship, providing additional details about the connection.
        /// </summary>
        public string? RelationshipDescription { get; set; }
        /// <summary>
        /// Gets or sets the related person associated with this entity.
        /// </summary>
        /// <remarks>The related person must be provided as it is a required property. Ensure that the
        /// provided Person object is valid and not null.</remarks>
        public required Person RelatedPerson { get; set; }
        public RelationshipBlock()
        {
            Type = BlockType.RelationshipBlock;
        }
        public override string ToString()
        {
            string formatter =
                $"Block - {Id}"
                + $"\n\tBrel- Type: {Type}"
                + $"\n\tBrel- RelationshipName: {RelationshipName}"
                + $"\n\tBrel- RelationshipDescription: {RelationshipDescription}"
                + $"\n\tBrel- RelatedPersonId: {(RelatedPerson != null ? RelatedPerson.Id.ToString() : "null")}";
            return formatter;
            //return base.ToString();
        }


    } // end of RelationshipBlock class
    public class ContactBlock : Block
    {
        /// <summary>
        /// Gets or sets the type of contact information, such as "Email", "Phone", or "Social Media", which indicates the nature of the contact details provided.
        /// </summary>
        public required string ContactType { get; set; }
        /// <summary>
        /// Gets or sets the contact information for the individual or entity.
        /// </summary>
        /// <remarks>This property is required and must contain a valid string representing the contact
        /// details. Ensure that the value is not null or empty when setting this property.</remarks>
        public required string ContactValue { get; set; }
        public ContactBlock()
        {
            Type = BlockType.ContactBlock;
        }
        public override string ToString()
        {
            string formatter =
                $"Block - {Id}"
                + $"\n\tBctc- Type: {Type}"
                + $"\n\tBctc- ContactType: {ContactType}"
                + $"\n\tBctc- ContactValue: {ContactValue}";
            return formatter;
            //return base.ToString();
        }


    } //end of ContactBlock class
}