using System;
using ABP.AppCore.Enums;
using ABP.Infrastructure.Factory.Implementation;

namespace ABP.Infrastructure.Factory.Base
{
    public static class ExperimentFactory
    {
        public static IExperirementator CreateExperiment(ExperimentType type)
        {
            switch (type)
            {
                case ExperimentType.ButtonExperiment:
                    return new ButtonExperiment();

                case ExperimentType.PriceExperiment:
                    return new PriceExperiment();

                default:
                    throw new ArgumentException("Unsupported  type");
            }
        }
    }
}
