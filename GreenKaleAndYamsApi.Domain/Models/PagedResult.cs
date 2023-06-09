﻿using System.Collections.Generic;

namespace GreenKaleAndYamsApi.Domain.Models {
	public class PagedResult<T> {
		public List<T> Results { get; set; }
		public int TotalResults { get; set; }
		public int TotalPages { get; set; }
		public int Page { get; set; }
	}
}
