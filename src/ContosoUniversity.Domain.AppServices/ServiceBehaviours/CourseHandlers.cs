﻿namespace ContosoUniversity.Domain.AppServices.ServiceBehaviours
{
    using ContosoUniversity.Core.Domain.ContextualValidation;
    using ContosoUniversity.Domain.Core.Behaviours.Courses;
    using Core.Factories;
    using Core.Repository.Containers;
    using Core.Repository.Entities;
    using NRepository.Core;
    using NRepository.EntityFramework;

    public static class CourseHandlers
    {
        // Create course
        public static CourseCreate.Response Handle(IRepository repository, CourseCreate.Request request)
        {
            // Validation now performed in the dispacther decorators (See AutoValidate<T> in the DomainBootstrapper class)

            var container = new EntityStateWrapperContainer();
            container.AddEntity(CourseFactory.Create(request.CommandModel));
            var validationDetails = repository.Save(container);

            var courseId = default(int?);
            if (!validationDetails.HasValidationIssues)
                courseId = container.FindEntity<Course>().CourseID;

            return new CourseCreate.Response(validationDetails, courseId);
        }

        // Update course credits
        public static CourseUpdateCredits.Response Handle(IRepository repository, CourseUpdateCredits.Request request)
        {
            var rowsAffected = repository.ExecuteStoredProcudure(
                "UPDATE Course SET Credits = Credits * {0}",
                request.CommandModel.Multiplier);

            return new CourseUpdateCredits.Response(rowsAffected);
        }

        // Update course 
        public static CourseUpdate.Response Handle(IRepository repository, CourseUpdate.Request request)
        {
            var container = CourseFactory.CreatePartial(request.CommandModel.CourseID).Modify(request.CommandModel);
            var validationDetails = repository.Save(container);

            return new CourseUpdate.Response(validationDetails);
        }

        // Delete course 
        public static CourseDelete.Response Handle(IRepository repository, CourseDelete.Request request)
        {
            var course = CourseFactory.CreatePartial(request.CommandModel.CourseId);
            var container = course.Delete();
            repository.Save(container);

            return new CourseDelete.Response();
        }
    }
}
