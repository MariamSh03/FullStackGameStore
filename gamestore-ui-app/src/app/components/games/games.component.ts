import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { GameService, Game, GameSearchParams } from '../../services/game.service';
import { GenreService, Genre } from '../../services/genre.service';
import { PublisherService, Publisher } from '../../services/publisher.service';
import { PlatformService, Platform } from '../../services/platform.service';
import { ApiConfigService } from '../../services/api-config.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { BehaviorSubject, forkJoin } from 'rxjs';

@Component({
  selector: 'app-games',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, TranslatePipe],
  templateUrl: './games.component.html',
  styleUrls: ['./games.component.css']
})

export class GamesComponent implements OnInit {
  games: Game[] = [];
  filteredGames: Game[] = [];
  loading = new BehaviorSubject<boolean>(false);
  error: string | null = null;
  
  // Pagination
  currentPage = 1;
  totalPages = 1;
  pageSize = 8;
  totalCount = 0;

  private gameImages = [
    'assets/photos/Leonardo_Phoenix_10_a_lush_3D_render_of_a_computer_game_cover_1.jpg',
    'assets/photos/Leonardo_Phoenix_10_a_lush_3D_render_of_a_computer_game_cover_3.jpg',
    'assets/photos/Lucid_Origin_generate_a_futuristic_computer_game_cover_photo_w_0.jpg',
    'assets/photos/Lucid_Origin_generate_a_futuristic_computer_game_cover_photo_w_1.jpg',
    'assets/photos/Lucid_Origin_generate_a_futuristic_computer_game_cover_photo_w_2.jpg',
    'assets/photos/Lucid_Origin_generate_a_futuristic_computer_game_cover_photo_w_3.jpg',
    'assets/photos/Lucid_Origin_generate_a_vibrant_computer_game_cover_photo_feat_0.jpg',
    'assets/photos/Lucid_Origin_generate_a_vibrant_computer_game_cover_photo_feat_1.jpg',
    'assets/photos/Lucid_Origin_generate_a_vibrant_computer_game_cover_photo_feat_2.jpg',
    'assets/photos/Lucid_Origin_generate_a_vibrant_computer_game_cover_photo_feat_3.jpg'
  ];

  // Track image loading errors and successful loads
  private imageErrors: Set<string> = new Set();
  private imageLoaded: Set<string> = new Set();



  constructor(
    private gameService: GameService,
    private genreService: GenreService,
    private publisherService: PublisherService,
    private platformService: PlatformService,
    private apiConfigService: ApiConfigService
  ) {}

  ngOnInit() {
    console.log('🎮 GamesComponent initializing...');
    
    // Wait for API configuration to be loaded before making any API calls
    this.apiConfigService.waitForConfig().subscribe(() => {
      console.log('✅ API configuration ready, loading data...');
      this.loadGames();
      this.loadGenres();
      this.loadPublishers();
      this.loadPlatforms();
    });
  }
  
  // Filter options - store IDs for backend, names for UI
  selectedGenreIds: string[] = [];
  selectedPlatformIds: string[] = [];
  selectedPublisherNames: string[] = [];
  priceRange = { min: 0, max: 100 };
  selectedRating = 0;
  searchQuery = '';
  sortBy = 'popular';
  priceFrom = '0';
  priceTo = '100';

  // Data loaded from backend
  genres: Genre[] = [];
  platforms: Platform[] = [];
  publishers: Publisher[] = [];
  
  // For UI display - we'll map these from backend data
  genreNames: string[] = [];
  platformNames: string[] = [];
  publisherNames: string[] = [];

  getDiscountedPrice(price: number, discount: number): number {
    return price - (price * discount / 100);
  }

  getRandomGameImage(gameId?: string): string {
    // Use game ID to ensure consistent image for the same game across renders
    const seed = gameId ? this.hashCode(gameId) : Math.random();
    const index = Math.abs(seed) % this.gameImages.length;
    return this.gameImages[index];
  }

  private hashCode(str: string): number {
    let hash = 0;
    for (let i = 0; i < str.length; i++) {
      const char = str.charCodeAt(i);
      hash = ((hash << 5) - hash) + char;
      hash = hash & hash; // Convert to 32bit integer
    }
    return hash;
  }

  onImageLoad(gameId: string): void {
    this.imageLoaded.add(gameId);
  }

  onImageError(event: any, gameId: string): void {
    this.imageErrors.add(gameId);
    console.warn(`Failed to load image for game ${gameId}:`, event);
  }

  getImageErrorStatus(gameId: string): boolean {
    return this.imageErrors.has(gameId);
  }

  getImageLoadStatus(gameId: string): boolean {
    return this.imageLoaded.has(gameId);
  }

  getGenreId(genreName: string): string {
    const genre = this.genres.find(g => g.name === genreName);
    return genre ? genre.id : '';
  }

  getPlatformId(platformName: string): string {
    const platform = this.platforms.find(p => p.type === platformName);
    return platform ? platform.id || '' : '';
  }

  getStars(rating: number): string {
    const fullStars = Math.floor(rating);
    const hasHalfStar = rating % 1 >= 0.5;
    return '★'.repeat(fullStars) + (hasHalfStar ? '☆' : '') + '☆'.repeat(5 - fullStars - (hasHalfStar ? 1 : 0));
  }



  // All filtering and sorting is handled by the backend

  loadGames() {
    this.loading.next(true);
    this.error = null;

    // Parse price from string inputs
    const minPrice = this.parsePrice(this.priceFrom);
    const maxPrice = this.parsePrice(this.priceTo);

    const searchParams: any = {
      page: this.currentPage,
      pageCount: this.pageSize,
      sort: this.sortBy,
      name: this.searchQuery || undefined,
      minPrice: minPrice,
      maxPrice: maxPrice,
      genres: this.selectedGenreIds.length > 0 ? this.selectedGenreIds : undefined,
      platforms: this.selectedPlatformIds.length > 0 ? this.selectedPlatformIds : undefined,
      publisherNames: this.selectedPublisherNames.length > 0 ? this.selectedPublisherNames : undefined
    };

    // Debug logging
    console.log('🔍 Loading games with params:', searchParams);
    console.log('📄 Current page before API call:', this.currentPage);

    this.gameService.getAllGames(searchParams).subscribe({
      next: (response) => {
        console.log('✅ API Response:', response);
        console.log('📊 Games returned:', response.data?.length || 0);
        console.log('📈 Total pages:', response.totalPages);
        console.log('📍 Current page from response:', response.currentPage);
        
        this.games = response.data;
        this.filteredGames = [...this.games];
        this.totalCount = response.totalCount;
        this.totalPages = response.totalPages;
        this.currentPage = response.currentPage;
        this.loading.next(false);
        
        // No need for local filters since backend handles filtering
      },
      error: (error) => {
        console.error('Failed to load games:', error);
        this.error = 'Failed to load games from server. Please check your connection.';
        this.games = [];
        this.filteredGames = [];
        this.loading.next(false);
      }
    });
  }

  loadGenres() {
    this.genreService.getAllGenres().subscribe({
      next: (genres) => {
        this.genres = genres;
        // Extract unique genre names for the filter UI
        this.genreNames = [...new Set(genres.map(g => g.name))].sort();
      },
      error: (error) => {
        console.error('Failed to load genres:', error);
        // Fallback to static genres if API fails
        this.genreNames = ['Action', 'RPG', 'FPS', 'Sports', 'Strategy', 'Racing', 'Adventure', 'Puzzle & Skill'];
      }
    });
  }

  loadPublishers() {
    this.publisherService.getAllPublishers().subscribe({
      next: (publishers) => {
        this.publishers = publishers;
        // Extract publisher company names for the filter UI
        this.publisherNames = publishers.map(p => p.companyName).sort();
      },
      error: (error) => {
        console.error('Failed to load publishers:', error);
        // Fallback to static publishers if API fails
        this.publisherNames = ['EA Games', 'Blizzard', 'Ubisoft', 'Activision'];
      }
    });
  }

  loadPlatforms() {
    console.log('🎮 Loading platforms from backend...');
    this.platformService.getAllPlatforms().subscribe({
      next: (platforms) => {
        console.log('✅ Platforms loaded from backend:', platforms);
        this.platforms = platforms;
        // Extract platform types for the filter UI
        this.platformNames = platforms.map(p => p.type).sort();
        console.log('📋 Platform names for UI:', this.platformNames);
      },
      error: (error) => {
        console.error('❌ Failed to load platforms from backend:', error);
        console.error('🔧 Using fallback static platforms');
        // Fallback to static platforms if API fails
        this.platformNames = ['PC', 'PlayStation', 'Xbox', 'Nintendo Switch'];
      }
    });
  }

  parsePrice(priceString: string): number {
    // Remove dollar sign and parse as number
    const cleaned = priceString.replace(/[$,]/g, '');
    const parsed = parseFloat(cleaned);
    return isNaN(parsed) ? 0 : parsed;
  }

  // Backend handles all filtering, no local filtering needed

  onSearchChange() {
    // Debounce search to avoid too many API calls
    clearTimeout(this.searchTimeout);
    this.searchTimeout = setTimeout(() => {
      this.currentPage = 1;
      this.loadGames();
    }, 500);
  }

  onSortChange() {
    this.currentPage = 1;
    this.loadGames();
  }

  onGenreChange(genreName: string, event: any) {
    const genre = this.genres.find(g => g.name === genreName);
    if (!genre) return;

    if (event.target.checked) {
      this.selectedGenreIds.push(genre.id);
    } else {
      this.selectedGenreIds = this.selectedGenreIds.filter(id => id !== genre.id);
    }
    this.currentPage = 1;
    this.loadGames();
  }

  onPlatformChange(platformName: string, event: any) {
    const platform = this.platforms.find(p => p.type === platformName);
    if (!platform) return;

    if (event.target.checked) {
      this.selectedPlatformIds.push(platform.id || '');
    } else {
      this.selectedPlatformIds = this.selectedPlatformIds.filter(id => id !== platform.id);
    }
    this.currentPage = 1;
    this.loadGames();
  }

  onPublisherChange(publisherName: string, event: any) {
    if (event.target.checked) {
      this.selectedPublisherNames.push(publisherName);
    } else {
      this.selectedPublisherNames = this.selectedPublisherNames.filter(p => p !== publisherName);
    }
    this.currentPage = 1;
    this.loadGames();
  }

  onRatingChange(rating: number) {
    this.selectedRating = rating;
    // Note: Rating filtering is not implemented in backend yet
    // Could be added later if needed
  }

  onPriceChange() {
    // Debounce price changes
    clearTimeout(this.priceTimeout);
    this.priceTimeout = setTimeout(() => {
      this.currentPage = 1;
      this.loadGames();
    }, 1000);
  }

  clearFilters() {
    this.selectedGenreIds = [];
    this.selectedPlatformIds = [];
    this.selectedPublisherNames = [];
    this.priceRange = { min: 0, max: 100 };
    this.selectedRating = 0;
    this.searchQuery = '';
    this.sortBy = 'popular';
    this.priceFrom = '0';
    this.priceTo = '100';
    this.currentPage = 1;
    this.loadGames();
  }

  // Pagination methods
  goToPage(page: number) {
    console.log('🎯 goToPage called with:', page);
    console.log('📊 Current state - currentPage:', this.currentPage, 'totalPages:', this.totalPages);
    
    if (page >= 1 && page <= this.totalPages) {
      console.log('✅ Valid page, setting currentPage to:', page);
      this.currentPage = page;
      this.loadGames();
    } else {
      console.warn('❌ Invalid page:', page, 'Valid range: 1 -', this.totalPages);
    }
  }

  previousPage() {
    this.goToPage(this.currentPage - 1);
  }

  nextPage() {
    this.goToPage(this.currentPage + 1);
  }

  private searchTimeout: any;
  private priceTimeout: any;
}
