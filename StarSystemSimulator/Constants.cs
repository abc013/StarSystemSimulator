namespace StarSystemSimulator
{
	public static class Constants
	{
		public const float Mass_Unit = 1.989e33f;
		public const float Time_Unit = 3.1556926e7f;
		public const float Dist_Unit = 1.495978707e13f;
		public const float Velocity_Unit = Dist_Unit / Time_Unit;

		// Gravitational Constant
		public const double GConst = 6.674e-8f * Mass_Unit * ((double)Time_Unit * Time_Unit) / ((double)Dist_Unit * Dist_Unit * Dist_Unit);
	}
}
