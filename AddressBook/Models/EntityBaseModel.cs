using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddressBook.Models
{
    /// <summary>
    /// Model class from which model classes will be inherited. Every model class that inherits from this one
    /// will have ID as primary key, CreatedDate, ModifiedDate
    /// </summary>
    public abstract class EntityBaseModel
    {
        public int ID { get; set; }

        /// <summary>
        /// This property will be updated when entity is created in overriden SaveChanges method
        /// </summary>        
        [ScaffoldColumn(false)]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// This property will be updated when entity is edited in overriden SaveChanges method
        /// </summary>
        [ScaffoldColumn(false)]
        public DateTime ModifiedAt { get; set; }
    }
}