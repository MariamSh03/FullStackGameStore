import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { GenreService, Genre } from '../../services/genre.service';

@Component({
  selector: 'app-genres',
  imports: [CommonModule, TranslateModule],
  templateUrl: './genres.component.html',
  styleUrl: './genres.component.css'
})
export class GenresComponent implements OnInit {
  genres: Genre[] = [];
  loading = false;
  error: string | null = null;

  // Group genres by parent for better display
  parentGenres: Genre[] = [];
  childGenresByParent: { [parentId: string]: Genre[] } = {};

  constructor(private genreService: GenreService) {}

  ngOnInit() {
    this.loadGenres();
  }

  loadGenres() {
    this.loading = true;
    this.error = null;

    this.genreService.getAllGenres().subscribe({
      next: (genres) => {
        this.genres = genres;
        this.organizeGenres();
        this.loading = false;
      },
      error: (error) => {
        console.error('Failed to load genres:', error);
        this.error = 'Failed to load genres from server. Please check your connection.';
        this.loading = false;
      }
    });
  }

  organizeGenres() {
    // Separate parent genres (no parentGenreId) from child genres
    this.parentGenres = this.genres.filter(genre => !genre.parentGenreId);
    
    // Group child genres by their parent
    this.childGenresByParent = {};
    const childGenres = this.genres.filter(genre => genre.parentGenreId);
    
    childGenres.forEach(genre => {
      if (genre.parentGenreId) {
        if (!this.childGenresByParent[genre.parentGenreId]) {
          this.childGenresByParent[genre.parentGenreId] = [];
        }
        this.childGenresByParent[genre.parentGenreId].push(genre);
      }
    });
  }

  getChildGenres(parentId: string): Genre[] {
    return this.childGenresByParent[parentId] || [];
  }
}