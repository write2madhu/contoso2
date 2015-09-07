namespace ContosoUniversity.Core.Domain
{
    using ContosoUniversity.Core.Domain.ContextualValidation;

    public abstract class DomainResponse
    {
        protected DomainResponse(ValidationMessageCollection validationDetails)
        {
            ValidationDetails = validationDetails;
        }

        public DomainResponse()
            : this(new ValidationMessageCollection())
        {
        }

        public ValidationMessageCollection ValidationDetails
        {
            get;
        }

        public bool HasValidationIssues
        {
            get { return ValidationDetails.HasValidationIssues; }
        }
    }
}
