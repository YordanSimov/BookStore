﻿namespace ProjectDK.Models.Configurations
{
    public class KafkaConsumerSettings
    {
        public string BootstrapServers { get; set; }

        public string GroupId { get; set; }

        public string CacheName { get; set; }

        public string DeliveryTopic { get; set; }

        public string PurchaseTopic { get; set; }
    }
}