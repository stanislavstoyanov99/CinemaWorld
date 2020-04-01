namespace CinemaWorld.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Data.Common.Repositories;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Models.InputModels.AdministratorInputModels.About;
    using CinemaWorld.Models.ViewModels.About;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    public class AboutService : IAboutService
    {
        private readonly IRepository<FaqEntry> faqEntriesRepository;

        public AboutService(IRepository<FaqEntry> faqEntriesRepository)
        {
            this.faqEntriesRepository = faqEntriesRepository;
        }

        public async Task<FaqDetailsViewModel> CreateAsync(FaqCreateInputModel faqCreateInputModel)
        {
            var faq = new FaqEntry
            {
                Question = faqCreateInputModel.Question,
                Answer = faqCreateInputModel.Answer,
            };

            bool doesFaqExist = await this.faqEntriesRepository.All().AnyAsync(x => x.Id == faq.Id);
            if (doesFaqExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.FaqAlreadyExists, faq.Id));
            }

            await this.faqEntriesRepository.AddAsync(faq);
            await this.faqEntriesRepository.SaveChangesAsync();

            var viewModel = this.GetViewModelByIdAsync<FaqDetailsViewModel>(faq.Id)
                .GetAwaiter()
                .GetResult();

            return viewModel;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var faq = this.faqEntriesRepository.All().FirstOrDefault(x => x.Id == id);
            if (faq == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.FaqNotFound, id));
            }

            faq.IsDeleted = true;
            faq.DeletedOn = DateTime.UtcNow;
            this.faqEntriesRepository.Update(faq);
            await this.faqEntriesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(FaqEditViewModel faqEditViewModel)
        {
            var faq = this.faqEntriesRepository.All().FirstOrDefault(g => g.Id == faqEditViewModel.Id);

            if (faq == null)
            {
                throw new NullReferenceException(
                    string.Format(ExceptionMessages.FaqNotFound, faqEditViewModel.Id));
            }

            faq.Answer = faqEditViewModel.Answer;
            faq.Question = faqEditViewModel.Question;
            faq.ModifiedOn = DateTime.UtcNow;

            this.faqEntriesRepository.Update(faq);
            await this.faqEntriesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllFaqsAsync<TModel>()
        {
            var faqs = await this.faqEntriesRepository
                .All()
                .To<TModel>()
                .ToListAsync();

            return faqs;
        }

        public async Task<TViewModel> GetViewModelByIdAsync<TViewModel>(int id)
        {
            var faqViewModel = await this.faqEntriesRepository
               .All()
               .Where(m => m.Id == id)
               .To<TViewModel>()
               .FirstOrDefaultAsync();

            if (faqViewModel == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.FaqNotFound, id));
            }

            return faqViewModel;
        }
    }
}
