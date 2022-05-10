using System;

namespace Songbird.Web.Exceptions;

public class ElasticsearchException : InvalidOperationException {
    public ElasticsearchException() {
    }

    public ElasticsearchException(string message) : base(message) {
    }

    public ElasticsearchException(string message, Exception innerException) : base(message, innerException) {
    }
}
